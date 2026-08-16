using CRUD.CommonLayer.Models;
using CRUD.CommonUtility;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CRUD.RepositoryLayer
{
    public class CrudAppliactionRL : ICrudAppliactionRL
    {
        private readonly IConfiguration _configuration;
        private readonly MultiTenant.ITenantProvider _tenantProvider;
        private readonly MultiTenant.ITenantsStore _tenantsStore;
        private const int ConnectionTimeOut = 180;

        public CrudAppliactionRL(IConfiguration configuration, MultiTenant.ITenantProvider tenantProvider, MultiTenant.ITenantsStore tenantsStore)
        {
            _configuration = configuration;
            _tenantProvider = tenantProvider;
            _tenantsStore = tenantsStore;
        }

        private SqlConnection CreateSqlConnection()
        {
            var connStr = _configuration["ConnectionStrings:SqlServerDBConnection"];
            if (!string.IsNullOrEmpty(_tenantProvider?.TenantId) && _tenantsStore != null)
            {
                if (_tenantsStore.TryGetTenant(_tenantProvider.TenantId, out var tenant) && !string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    connStr = tenant.ConnectionString;
                }
            }

            return new SqlConnection(connStr);
        }

        public async Task<CreateInformationResponse> CreateInformation(CreateInformationRequest request)
        {
            var resposne = new CreateInformationResponse { IsSuccess = true, Message = "Successful" };
            try
            {
                using var conn = CreateSqlConnection();
                string StoreProcedure = "SpCreateInformation";
                using var sqlCommand = new SqlCommand(StoreProcedure, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandTimeout = ConnectionTimeOut
                };

                sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                sqlCommand.Parameters.AddWithValue("@Age", request.Age);
                await conn.OpenAsync();
                int Status = await sqlCommand.ExecuteNonQueryAsync();
                if (Status <= 0)
                {
                    resposne.IsSuccess = false;
                    resposne.Message = "CreateInformation Not Executed";
                }
            }
            catch (Exception ex)
            {
                resposne.IsSuccess = false;
                resposne.Message = "Exception Message : " + ex.Message;
            }

            return resposne;
        }

        public async Task<ReadInformationResponse> ReadInformation()
        {
            var response = new ReadInformationResponse
            {
                readInformation = new List<ReadInformation>(),
                IsSuccess = true,
                Message = "Successful"
            };

            try
            {
                string StoreProcedure = "SpReadInformation";
                using var conn = CreateSqlConnection();
                using var sqlCommand = new SqlCommand(StoreProcedure, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandTimeout = ConnectionTimeOut
                };

                await conn.OpenAsync();
                using var reader = await sqlCommand.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var getResponse = new ReadInformation
                        {
                            UserID = reader["ID"] != DBNull.Value ? Convert.ToInt32(reader["ID"]) : 0,
                            UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                            Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0
                        };
                        response.readInformation.Add(getResponse);
                    }
                }
                else
                {
                    response.Message = "No data Return";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }

        public async Task<UpdateInformationResponse> UpdateInformation(UpdateInformationRequest request)
        {
            var resposne = new UpdateInformationResponse { IsSuccess = true, Message = "Successful" };
            try
            {
                using var conn = CreateSqlConnection();
                string StoreProcedure = "SpUpdateInformation";
                using var sqlCommand = new SqlCommand(StoreProcedure, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandTimeout = ConnectionTimeOut
                };

                sqlCommand.Parameters.AddWithValue("@Id", request.UserId);
                sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                sqlCommand.Parameters.AddWithValue("@Age", request.Age);
                await conn.OpenAsync();
                int Status = await sqlCommand.ExecuteNonQueryAsync();
                if (Status <= 0)
                {
                    resposne.IsSuccess = false;
                    resposne.Message = "UpdateInformation Not Executed";
                }
            }
            catch (Exception ex)
            {
                resposne.IsSuccess = false;
                resposne.Message = "Exception Message : " + ex.Message;
            }

            return resposne;
        }

        public async Task<DeleteInformationResponse> DeleteInformation(DeleteInformationRequest request)
        {
            var resposne = new DeleteInformationResponse { IsSuccess = true, Message = "Successful" };
            try
            {
                using var conn = CreateSqlConnection();
                string StoreProcedure = "SpDeleteInformation";
                using var sqlCommand = new SqlCommand(StoreProcedure, conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandTimeout = ConnectionTimeOut
                };

                sqlCommand.Parameters.AddWithValue("@Id", request.UserId);
                await conn.OpenAsync();
                int Status = await sqlCommand.ExecuteNonQueryAsync();
                if (Status <= 0)
                {
                    resposne.IsSuccess = false;
                    resposne.Message = "UnSuccessful";
                }
            }
            catch (Exception ex)
            {
                resposne.IsSuccess = false;
                resposne.Message = "Exception Message : " + ex.Message;
            }

            return resposne;
        }

        public async Task<SearchInformationByIdResponse> SearchInformationById(SearchInformationByIdRequest request)
        {
            var response = new SearchInformationByIdResponse { IsSuccess = true, Message = "Successful" };
            try
            {
                using var conn = CreateSqlConnection();
                using var sqlCommand = new SqlCommand(SqlQueries.ReadInformation, conn)
                {
                    CommandType = System.Data.CommandType.Text,
                    CommandTimeout = ConnectionTimeOut
                };

                sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
                await conn.OpenAsync();
                using var reader = await sqlCommand.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    response.searchInformationById = new SearchInformationById
                    {
                        UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                        Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0
                    };
                }
                else
                {
                    response.Message = "No data Found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Message : " + ex.Message;
            }

            return response;
        }
    }
}
