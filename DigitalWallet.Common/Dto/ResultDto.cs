using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWallet.Common.Dto
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
    }
    public class ResultDto<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
    }
}
