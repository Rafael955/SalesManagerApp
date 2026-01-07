using FluentValidation;
using SalesManagerApp.Domain.Dtos.Requests;
using SalesManagerApp.Domain.Dtos.Responses;
using SalesManagerApp.Domain.Entities;
using SalesManagerApp.Domain.Interfaces.Repositories;
using SalesManagerApp.Domain.Interfaces.Services;
using SalesManagerApp.Domain.Mappers;
using SalesManagerApp.Domain.Validations;

namespace SalesManagerApp.Domain.Services
{
    public class CustomerDomainService(ICustomerRepository customerRepository) : ICustomerDomainService
    {
        public CustomerResponseDto RegistrarCliente(CustomerRequestDto request)
        {
            var validation = new CustomerValidator().Validate(request);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);


            if (customerRepository.GetCustomerByEmail(request.Email!) != null)
                throw new ApplicationException("Já existe um cliente cadastrado com este e-mail.");

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow
            };

            customerRepository.Add(customer);

            return customer.MapToResponse();
        }

        public CustomerResponseDto AtualizarCliente(Guid id, CustomerRequestDto request)
        {
            var customer = customerRepository.GetById(id);

            if (customer == null)
                throw new ApplicationException("O cliente com este Id não existe!");

            var validation = new CustomerValidator().Validate(request);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var customerByEmail = customerRepository.GetCustomerByEmail(request.Email!);

            if (customerByEmail != null && customerByEmail.Id != id)
                throw new ApplicationException("Já existe um cliente cadastrado com este e-mail.");

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Phone = request.Phone;

            customerRepository.Update(customer);

            return customer.MapToResponse();
        }

        public void ExcluirCliente(Guid id)
        {
            var customer = customerRepository.GetById(id);

            if (customer == null)
                throw new ApplicationException("O cliente com este Id não existe!");

            customerRepository.Delete(customer);
        }

        public CustomerResponseDto ObterClientePorId(Guid id)
        {
            var customer = customerRepository.GetById(id);

            if (customer == null)
                throw new ApplicationException("O cliente com este Id não existe!");

            return customer.MapToResponse();
        }

        public ICollection<CustomerResponseDto> ObterTodosClientes()
        {
            return customerRepository.GetAll().Select(customer => customer.MapToResponse()).ToList();
        }
    }
}
