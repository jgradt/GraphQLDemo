﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models;
using WebApiDemo.Data;
using AutoMapper;
using WebApiDemo.Infrastructure;
using WebApiDemo.Data.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApiDemo.Data.Dto;
using WebApiDemo.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<PagedData<CustomerDto>> Get(int pageIndex = 0, int pageSize = 10)
        {
            var data = await customerRepository.GetPagedAsync(pageIndex, pageSize);
            var mappedData = new PagedData<CustomerDto>()
            {
                PageIndex = data.PageIndex,
                PageSize = data.PageSize,
                TotalItems = data.TotalItems,
                Items = mapper.Map<List<CustomerDto>>(data.Items)
            };

            return mappedData;
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            if(customer == null)
            {
                return NotFound();
            }

            var mappedData = mapper.Map<CustomerDto>(customer);

            return new ObjectResult(mappedData);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDto item)
        {
            var mappedDataIn = mapper.Map<Customer>(item);
            var result =  await customerRepository.AddAsync(mappedDataIn);
            await customerRepository.SaveAsync();
            var mappedDataOut = mapper.Map<CustomerDto>(result);

            return CreatedAtRoute("GetCustomer", new { id = mappedDataOut.Id }, mappedDataOut);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]CustomerDto item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var mappedData = mapper.Map<Customer>(item);

            await customerRepository.UpdateAsync(id, mappedData);
            await customerRepository.SaveAsync();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await customerRepository.DeleteAsync(id);
            await customerRepository.SaveAsync();

            return new NoContentResult();
        }
    }
}
