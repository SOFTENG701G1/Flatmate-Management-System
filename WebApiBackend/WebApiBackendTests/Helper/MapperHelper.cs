using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiBackend.Dto;
using WebApiBackend.Model;

namespace WebApiBackendTests.Helper
{
    class MapperHelper
    {
        private readonly IMapper _mapper;

        public MapperHelper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDTO, Payment>();
                cfg.CreateMap<Payment, PaymentDTO>();
            });

            _mapper = config.CreateMapper();
        }

        public IMapper GetMapper()
        {
            return _mapper;
        }
    }
}
