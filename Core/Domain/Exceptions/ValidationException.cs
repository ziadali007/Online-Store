﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ValidationException(IEnumerable<string> errors): Exception("Validation errors occurred")
    {
        public IEnumerable<string> Errors { get; } = errors;
    }
    
}
