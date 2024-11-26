using Cultivatrade.Api.DatabaseConnection;
using Cultivatrade.Api.DTO;
using Cultivatrade.Api.Models;
using Cultivatrade.Api.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using DrawingImage = System.Drawing.Image;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using System.Drawing;
using System.Drawing.Imaging;
using BC = BCrypt.Net.BCrypt;

namespace Cultivatrade.Api.Logics
{
    public class UserLogic
    {
        private readonly CultivatradeContext _context;
        private readonly Base64Resizer _base64Resizer;
        private readonly FilePath _filePath;
        public Random random = new Random();
        public readonly EmailSender _emailSender;

        public UserLogic(CultivatradeContext context, EmailSender emailSender, Base64Resizer base64Resizer, FilePath filePath)
        {
            _context = context;
            _emailSender = emailSender;
            _base64Resizer = base64Resizer;
            _filePath = filePath;
        }

        // RESET PASSWORD
        public bool ResetPassword(string email, string password)
        {
            int success = 0;
            int verificationCode = random.Next(1000, 9999);

            var data = CheckUserByEmail(email);
            data.Password = BC.HashPassword(password, BC.GenerateSalt());
            data.VerificationCode = verificationCode;

            _context.Users.Update(data);
            success = _context.SaveChanges();

            if(success > 0)
            {
                return true;
            }
            return false;
        }
        
        // VERIFY CODE
        public User VerifyCode(string email, int verificationCode)
        {
            var data = _context.Users.FirstOrDefault(x=>x.Email == email && x.VerificationCode == verificationCode);
           
            if (data != null)
            {
                return data;
            }
            return null;
        }

        // SEND CODE 
        public User SendCode(string email)
        {
            int success = 0;
            
            int verificationCode = random.Next(1000, 9999);

            var data = CheckUserByEmail(email);
            if(data != null)
            {
                data.VerificationCode = verificationCode;
                _context.Users.Update(data);
                success = _context.SaveChanges();

                if (success > 0)
                {
                    _emailSender.SendEmail(email, verificationCode);
                    return data;
                }
                return null;
            }
            return null;
        }

        public User SendRegistrationCode(string email, int verificationCode)
        {
            int success = 0;

            var data = CheckUserByEmail(email);
            if (data == null)
            {
                _emailSender.SendEmail(email, verificationCode);
                return data;
            }
            return null;
        }

        // CHECK USER
        public User CheckUserByEmail(string email)
        {
            var data = _context.Users.FirstOrDefault(x => x.Email == email);
            return data;
        }

        // LOGIN USER
        public UserDTO_GET LoginUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x=>x.Email == email);

            if (user != null && BC.Verify(password, user.Password))
            {

                var data = new UserDTO_GET
                {
                    UserId = user.UserId,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Phone = user.Phone,
                    Email = user.Email,
                    Address = user.Address,
                    IsSeller = user.IsSeller,
                    IsApproved = user.IsApproved,
                    BusinessPermitNumber = user.BusinessPermitNumber,
                    ProfileDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(user.ProfileImage)),
                    BusinessPermitDataUrl = _base64Resizer.ResizeImage(_filePath.BusinessPermitImage(user.BusinessPermitImage)),
                    SanitaryPermitDataUrl = _base64Resizer.ResizeImage(_filePath.SanitaryImage(user.SanitaryPermitImage)),
                    Password = password
                };

                return data;
            }
            return null;
        }

        // GET USER BY USER ID
        public UserDTO_GET GetUserByUserId(Guid userId)
        {
            var data = (from u in _context.Users
                        where u.UserId == userId
                        select new UserDTO_GET
                        {
                            UserId = u.UserId,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Email = u.Email,
                            Address = u.Address,
                            IsSeller = u.IsSeller,
                            IsApproved = u.IsApproved,
                            BusinessPermitNumber = u.BusinessPermitNumber,
                            BusinessPermitDataUrl = _base64Resizer.ResizeImage(_filePath.BusinessPermitImage(u.BusinessPermitImage)),
                            SanitaryPermitDataUrl = _base64Resizer.ResizeImage(_filePath.SanitaryImage(u.SanitaryPermitImage)),
                            ProfileDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                        }).FirstOrDefault();

            
            return data;
        }

    // ADD USER
    public bool AddUser(UserDTO_POST dto)
    {
        int success = 0;
        string profileName = "";
        Guid userId = Guid.NewGuid();
        DateTime dateTimeCreated = DateTime.Now;

        if (dto.ProfileImage != null)
        {
            profileName = userId.ToString() + Path.GetExtension(dto.ProfileImage.FileName);
            var profilePath = _filePath.UserImage("");
            string profileFilePath = Path.Combine(profilePath, profileName);

            using (var image = ImageSharpImage.Load(dto.ProfileImage.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(200, 200)); 
                image.Save(profileFilePath);  
            }
        }
        
        var data = new User();
        data.UserId = userId;
        data.Firstname = dto.Firstname;
        data.Lastname = dto.Lastname;
        data.Phone = dto.Phone;
        data.Email = dto.Email;
        data.Password = BC.HashPassword(dto.Password, BC.GenerateSalt());
        data.Address = dto.Address;
        data.ProfileImage = profileName;
        data.IsSeller = false;
        data.IsApproved = false;
        data.DateTimeCreated = dateTimeCreated;

        _context.Users.Add(data);
        success = _context.SaveChanges();

        if (success > 0)
        {
            var userAddress = new UserAddress
            {
                UserAddressId = Guid.NewGuid(),
                UserId = userId,
                Address = dto.Address,
                DateTimeCreated = dateTimeCreated
            };

            _context.UserAddresses.Add(userAddress);
            _context.SaveChanges();
            return true;
        }
        return false;
    }


        // UPDATE USER (NOT FIX YET)
        public bool UpdateUser(Guid userId, UserDTO_PUT dto)
        {
            int success = 0;
            string profileName = "";
            string businessPermitName = "";
            string sanitaryPermitName = "";

            var data = _context.Users.FirstOrDefault(x => x.UserId == userId);

            // PROFILE
            if (dto.ProfileImage != null && dto.ProfileImage.Length > 0)
            {
                profileName = userId.ToString() + Path.GetExtension(dto.ProfileImage.FileName);
                data.ProfileImage = profileName;
                var profilePath = _filePath.UserImage("");
                string profileFilePath = Path.Combine(profilePath, profileName);

                using (var image = System.Drawing.Image.FromStream(dto.ProfileImage.OpenReadStream()))
                using (var resizedImage = new Bitmap(200, 200))
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, 0, 0, 200, 200);
                    resizedImage.Save(profileFilePath, ImageFormat.Png);
                }
            }

            // PASSWORD
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                data.Password = BC.HashPassword(dto.Password, BC.GenerateSalt());
            }

            data.Firstname = dto.Firstname;
            data.Lastname = dto.Lastname;
            data.Phone = dto.Phone;
            data.Address = dto.Address;

            _context.Users.Update(data);
            success = _context.SaveChanges();

            if (success > 0)
            {
                return true;
            }
            return false;
        }

        // SUBMIT SELLER REQUIREMENTS
        public bool SubmitSellerRequirements(Guid userId, UserDTO_PATCH dto)
        {
            int success = 0;
            string businessPermitName = "";
            string sanitaryPermitName = "";

            var data = _context.Users.FirstOrDefault(x => x.UserId == userId);

            // BUSINESS PERMIT
            businessPermitName = userId.ToString() + Path.GetExtension(dto.BusinessPermitImage.FileName);
            var businessPermitPath = _filePath.BusinessPermitImage("");
            string businessPermitFilePath = Path.Combine(businessPermitPath, businessPermitName);

            using (var businessPermitFileStream = new FileStream(businessPermitFilePath, FileMode.Create))
            {
                dto.BusinessPermitImage.CopyTo(businessPermitFileStream);
            }

            // SANITARY PERMIT
            //sanitaryPermitName = userId.ToString() + Path.GetExtension(dto.BusinessPermitImage.FileName);
            //var sanitaryPermitPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images/SanitaryPermits");
            //string sanitartPermitFilePath = Path.Combine(sanitaryPermitPath, sanitaryPermitName);

            //using (var sanitaryPermitFileStream = new FileStream(sanitartPermitFilePath, FileMode.Create))
            //{
            //    dto.SanitaryPermitImage.CopyTo(sanitaryPermitFileStream);
            //}

            //data.BusinessPermitNumber = dto.BusinessPermitNumber;
            data.BusinessPermitImage = businessPermitName;
            //data.SanitaryPermitImage = sanitaryPermitName;
            data.FarmName = dto.FarmName;
            data.FarmAddress = dto.FarmAddress;
            data.FarmDescription = dto.FarmDescription;
            data.IsSeller = true;

            _context.Update(data);
            success = _context.SaveChanges();
            if(success > 0)
            {
                return true;
            }
            return false;
        }

        // APPROVE / DISAPPROVE USER AS SELLER
        public bool ApproveDisapprove(Guid usersId)
        {
            int success = 0;
            var data = _context.Users.FirstOrDefault(x => x.UserId == usersId);
            data.IsApproved = !data.IsApproved;

            _context.Users.Update(data);
            success = _context.SaveChanges();

            if(success > 0)
            {
                return true;
            }
            return false;
        }

        public int GetSeller()
        {
            var data = _context.Users.Where(x=>x.IsSeller == true && x.IsApproved == true).ToList().Count();
            return data;
        }

        public int GetBuyer()
        {
            var data = _context.Users.Where(x => (x.IsSeller == false && x.IsApproved == false) || (x.IsSeller == true && x.IsApproved == false)).ToList().Count();
            return data;
        }
        public List<UserDTO_GET> GetAllBuyers(){
            var data = (from u in _context.Users
                        where u.IsSeller == false && u.IsApproved == false
                        select new UserDTO_GET
                        {
                            UserId = u.UserId,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Email = u.Email,
                            Address = u.Address,
                            IsApproved = u.IsApproved,
                            IsSeller = u.IsSeller,
                            BusinessPermitNumber = u.BusinessPermitNumber,
                            ProfileDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                            BusinessPermitDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(u.BusinessPermitImage)),
                            SanitaryPermitDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(u.SanitaryPermitImage))
                        }).ToList();
                        return data;
        }
        public List<UserDTO_GET> GetUserByStatus(bool isApproved)
        {
            var data = (from u in _context.Users
                        where u.IsSeller == true && u.IsApproved == isApproved
                        select new UserDTO_GET
                        {
                            UserId = u.UserId,
                            Firstname = u.Firstname,
                            Lastname = u.Lastname,
                            Phone = u.Phone,
                            Email = u.Email,
                            Address = u.Address,
                            IsApproved = u.IsApproved,
                            IsSeller = u.IsSeller,
                            BusinessPermitNumber = u.BusinessPermitNumber,
                            ProfileDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(u.ProfileImage)),
                            BusinessPermitDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(u.BusinessPermitImage)),
                            SanitaryPermitDataUrl = _base64Resizer.ResizeImage(_filePath.UserImage(u.SanitaryPermitImage))

                        }).ToList();
                        return data;
        }

        //private string GetResizedImageAsBase64(string imagePath, int width = 50, int height = 50)
        //{
        //    if (!File.Exists(imagePath)) return null;

        //    using (var image = SixLabors.ImageSharp.Image.Load(imagePath))
        //    {
        //        image.Mutate(x => x.Resize(width, height));

        //        using (var ms = new MemoryStream())
        //        {
        //            image.SaveAsPng(ms); // Save as PNG or any desired format
        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}



    }
}
