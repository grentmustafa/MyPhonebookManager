using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using PhoneBookManager.Domain.DomainImplementation;
using MyPhoneBookManager.Domain.Interfaces;
using MyPhoneBookManager.DTO;
using MyPhoneBookManager.Repository.Interfaces;
using MyPhoneBookManager.Repository.RepositoriesWithJson;

namespace MyPhoneBookManager.WebAPI.PhoneBookManager
{
    public static class PhoneBookManagerEndpoints
    {
        public static void MapPhoneBookManagerEndpoints(this WebApplication app)
        {
            app.MapGet("/GetPhoneBookRecordByUserId/{id}", GetPhoneBookRecordByUserId);
            app.MapGet("/GetAllPhoneBookRecords", GetAllPhoneBookRecords);
            app.MapPost("/GetAllPhoneBookRecordsSorted", GetAllPhoneBookRecordsSorted);
            app.MapPost("/AddPhoneBookRecord", AddPhoneBookRecord);
            app.MapDelete("/DeletePhoneBookRecord/{id}", DeletePhoneBookRecord);
            app.MapDelete("/DeletePhoneNumberOfUser/{id}", DeletePhoneNumberOfUser);
            app.MapPut("/UpdatePhoneNumber/{id}", UpdatePhoneNumber);
            app.MapPut("/UpdatePhoneBookRecord/{id}", UpdatePhoneBookRecord);
        }

        public static void AddPhoneBookManagerServices(this IServiceCollection services)
        {
            services.AddSingleton<IPhoneRecordsRepository, PhoneRecordsRepositoryJsonFile>();
            services.AddSingleton<IUserRepository, UserRepositoryJsonFile>();
            services.AddTransient<IPhoneBookManagerDomain, PhoneBookManagerDomain>();
        }
        [HttpGet("{id}")]
        internal static IResult GetPhoneBookRecordByUserId(IPhoneBookManagerDomain iphoneBookManagerDomain,long id)
        {
            var result = iphoneBookManagerDomain.GetPhoneBookRecordByUserId(id);
            return result != null ? Results.Ok(result): Results.NotFound();
        }

        [HttpGet()]
        internal static IResult GetAllPhoneBookRecords(IPhoneBookManagerDomain iphoneBookManagerDomain)
        {
            var result = iphoneBookManagerDomain.GetAllPhoneBookRecords();
            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        [HttpPost()]
        internal static IResult GetAllPhoneBookRecordsSorted(IPhoneBookManagerDomain iphoneBookManagerDomain,[FromBody] GetAllRecordsSorted request)
        {
            var result = iphoneBookManagerDomain.GetAllPhoneBookRecordsSorted(request.asc,request.firstNameOrder);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }
        [HttpPost()]
        internal static IResult AddPhoneBookRecord(IPhoneBookManagerDomain iphoneBookManagerDomain, [FromBody] UsersInDTO request)
        {
            var result = iphoneBookManagerDomain.AddPhoneBookRecord(request);
            return Results.Created($"/GetPhoneBookRecordByUserId/{result.ID}", result);
        }

        [HttpDelete("id")]
        internal static IResult DeletePhoneBookRecord(IPhoneBookManagerDomain iphoneBookManagerDomain, long id)
        {
            var deletedRecord = iphoneBookManagerDomain.DeletePhoneBookRecord(id);
            return deletedRecord!=null ? Results.Ok(deletedRecord): Results.NotFound();
        }

        [HttpDelete("id")]
        internal static IResult DeletePhoneNumberOfUser(IPhoneBookManagerDomain iphoneBookManagerDomain, long id)
        {
            var deletedRecord = iphoneBookManagerDomain.DeletePhoneNumberOfUser(id);
            return deletedRecord != null ? Results.Ok(deletedRecord) : Results.NotFound();
        }

        [HttpPut("id")]
        internal static IResult UpdatePhoneNumber(IPhoneBookManagerDomain iphoneBookManagerDomain,long id,[FromBody] PhoneNumberInDTO request)
        {
            var updatePhoneNumber = iphoneBookManagerDomain.UpdatePhoneNumber(id,request);
            return updatePhoneNumber != null ? Results.Ok(updatePhoneNumber) : Results.NotFound();
        }

        [HttpPut("id")]
        internal static IResult UpdatePhoneBookRecord(IPhoneBookManagerDomain iphoneBookManagerDomain, long id,[FromBody] PhoneRecordUpdateDTO request)
        {
            var updatePhoneNumber = iphoneBookManagerDomain.UpdatePhoneBookRecord(id, request);
            return updatePhoneNumber != null ? Results.Ok(updatePhoneNumber) : Results.NotFound();
        }

    }
}
