using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2PlayersSettings.Data.Repository.Model.APIResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> SuccessResult(T data, string message = "操作成功") =>
            new ApiResponse<T> { Success = true, Message = message, Data = data };

        public static ApiResponse<T> FailureResult(string message) =>
            new ApiResponse<T> { Success = false, Message = message };
    }
}
