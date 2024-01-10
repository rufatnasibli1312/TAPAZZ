using DAL.Data;
using DTO.ResponseDto_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Filter.ActionFilter
{
    public class StandardizeResponseFilter : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is ErrorResponseDto)
                {

                    return;
                }

                var statusCode = objectResult.StatusCode ?? 200;
                context.Result = new ObjectResult(new ApiResponse<object>
                {
                    StatusCode = statusCode,
                    Data = objectResult.Value,
                    Message = "Request successful",
                })
                {
                    StatusCode = statusCode
                };
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
        public class ApiResponse<T>
        {
            public int StatusCode { get; set; }
            public T Data { get; set; }
            public string Message { get; set; }
        }
    }
}
