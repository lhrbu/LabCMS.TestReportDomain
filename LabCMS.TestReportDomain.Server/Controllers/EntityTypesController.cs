using Dapper.Contrib.Extensions;
using LabCMS.TestReportDomain.Server.Services;
using LabCMS.TestReportDomain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntityTypesController:ControllerBase
    {
        private readonly EntityTypeLoader _typeLoader;
        private readonly DatabaseAccessService _database;

        public EntityTypesController(
            EntityTypeLoader assemblyLoader,
            DatabaseAccessService database)
        {
            _typeLoader = assemblyLoader;
            _database = database;
        }

        [HttpPost]
        public async ValueTask<object> SearchAsync(
            [FromQuery]int id,
            [FromBody] EntityType entityType)
        {
            Type type = _typeLoader.Get(entityType);
            MethodInfo methodInfo = typeof(SqlMapperExtensions).GetMethod("GetAsync",
               BindingFlags.Public | BindingFlags.Static)!.MakeGenericMethod(type);
            dynamic result = methodInfo.Invoke(null, new object[] { _database.DbConnection,id, null, null })!;
            return await result;
        }

        [HttpPost("All")]
        public async ValueTask<IEnumerable<object>> SearchAllAsync(EntityType entityType)
        {
            Type type = _typeLoader.Get(entityType);
            MethodInfo methodInfo= typeof(SqlMapperExtensions).GetMethod("GetAllAsync", 
                BindingFlags.Public | BindingFlags.Static)!.MakeGenericMethod(type);
            dynamic result = methodInfo.Invoke(null, new[] { _database.DbConnection, null, null })!;
            return  ((await result) as IEnumerable<object>)!;
        }

        [HttpPost("Insert")]
        public async ValueTask<IActionResult> PostAsync(
            [FromQuery]string typeFullName,
            [FromBody]JsonElement entity)
        {
            Type? type = _typeLoader.FindInCache(typeFullName);
            if(type is null) { return BadRequest($"{typeFullName} is not loaded yet!"); }
            MethodInfo methodInfo = typeof(SqlMapperExtensions).GetMethod("InsertAsync",
                BindingFlags.Public | BindingFlags.Static)!.MakeGenericMethod(type);
            
            object tentity = await DeserializeJsonElementAsync(entity,type);
            dynamic result = methodInfo.Invoke(null, new[] { _database.DbConnection, tentity,null, null, null })!;
            await result;
            return Ok();
        }

        [HttpPut]
        public async ValueTask<IActionResult> PutAsync(
            [FromQuery] string typeFullName,
            [FromBody] JsonElement entity)
        {
            Type? type = _typeLoader.FindInCache(typeFullName);
            if (type is null) { return BadRequest($"{typeFullName} is not loaded yet!"); }
            MethodInfo methodInfo = typeof(SqlMapperExtensions).GetMethod("UpdateAsync",
                BindingFlags.Public | BindingFlags.Static)!.MakeGenericMethod(type);

            object tentity = await DeserializeJsonElementAsync(entity, type);
            dynamic result = methodInfo.Invoke(null, new[] { _database.DbConnection, tentity, null, null })!;
            await result;
            return Ok();
        }

        [HttpDelete]
        public async ValueTask<IActionResult> DeleteAsync(
             [FromQuery] string typeFullName,
            [FromBody] JsonElement entity)
        {
            Type? type = _typeLoader.FindInCache(typeFullName);
            if (type is null) { return BadRequest($"{typeFullName} is not loaded yet!"); }
            MethodInfo methodInfo = typeof(SqlMapperExtensions).GetMethod("DeleteAsync",
                BindingFlags.Public | BindingFlags.Static)!.MakeGenericMethod(type);

            object tentity = await DeserializeJsonElementAsync(entity, type);
            dynamic result = methodInfo.Invoke(null, new[] { _database.DbConnection, tentity, null, null })!;
            await result;
            return Ok();

        }

        private async ValueTask<object> DeserializeJsonElementAsync(JsonElement value,Type type)
        {
            ArrayBufferWriter<byte> buffer = new();
            await using Utf8JsonWriter writer = new(buffer);
            value.WriteTo(writer);
            await writer.FlushAsync();
            return (JsonSerializer.Deserialize(buffer.WrittenSpan, type))!;
        }
    }
}
