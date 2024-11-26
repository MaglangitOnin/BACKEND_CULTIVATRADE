using Microsoft.EntityFrameworkCore;
using VisitorSystem.Api.Models;

namespace VisitorSystem.Api.Logics
{
    public class PaginationLogic
    {
        public static PaginationResponseWrapper<T> PaginateData<T>(List<T> query, PaginationRequest paginationRequest)
        {
            var totalElements = query.Count();
            var totalPages = (totalElements + paginationRequest.ElementsPerPage - 1) / paginationRequest.ElementsPerPage;

            var response = new PaginationResponseWrapper<T>()
            {
                CurrentPage = paginationRequest.CurrentPage,
                ElementsPerPage = paginationRequest.ElementsPerPage,
                TotalElements = totalElements,
                TotalPages = totalPages,
                Results = query.Skip((paginationRequest.CurrentPage - 1) * paginationRequest.ElementsPerPage).Take(paginationRequest.ElementsPerPage).ToList()
            };

            return response;
        }
    }
}
