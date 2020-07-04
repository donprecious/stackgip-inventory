using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using StackgipInventory.Dto;
using StackgipInventory.Shared;

namespace StackgipEcommerce.Shared
{
    public static class Responses
    {
        // todo create generic responses for Ok, Created, Unauthorize, BadRequest
        public static ResponseDto Ok(object obj, string status = ResponseStatus.Success, string message = "" )
        {
         
           return new ResponseDto()
           {
               Data = obj, 
               Status = status, 
               Message = message
           };
        }

        public static ResponseDto Delete(string message = "" )
        {
         
            return new ResponseDto()
            {
                Data = null, 
                Status = ResponseStatus.Success, 
                Message = message
            };
        }

        public static ResponseDto NotFound(string message = "resource(s) not found" )
        {
         
            return new ResponseDto()
            {
                Data = null, 
                Status = ResponseStatus.Fail, 
                Message = message, 
                Code = ResponseCode.NotFound.ToString()
            };
        }

        public static ResponseDto Deleted(string message = "resource(s) not deleted" )
        {
         
            return new ResponseDto()
            {
                Data = null, 
                Status = ResponseStatus.Success, 
                Message = message,
            };
        }
        public static ResponseDto Error(string message = "An error occured while handling your request.")
        {
            return new ResponseDto()
            {
                Data = null,
                Status = ResponseStatus.Fail,
                Message = message,
            };
        }
    }

    public enum ResponseCode
    {
        NotFound
    }
}
