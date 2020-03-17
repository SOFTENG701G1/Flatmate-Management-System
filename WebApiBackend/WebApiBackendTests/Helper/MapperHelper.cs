using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiBackend.Dto;
using WebApiBackend.Model;

namespace WebApiBackendTests.Helper
{
    class MapperHelper
    {
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

        public MapperHelper()
        {
            _mockMapper.Setup(x => x.Map<PaymentDTO>(It.IsAny<Payment>()))
                .Returns((Payment payment) =>
                {
                    return new PaymentDTO
                    {
                        Id = payment.Id,
                        Amount = payment.Amount,
                        Fixed = payment.Fixed,
                        StartDate = payment.StartDate,
                        EndDate = payment.EndDate,
                        PaymentType = payment.PaymentType,
                        Frequency = payment.Frequency,
                        Description = payment.Description,
                    };
                });
        }

        public IMapper GetMapper()
        {
            return _mockMapper.Object;
        }
    }
}
