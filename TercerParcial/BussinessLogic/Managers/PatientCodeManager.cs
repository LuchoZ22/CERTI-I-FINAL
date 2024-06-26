﻿using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text.Json;
using UPB.BussinessLogic.Managers.Exceptions;
using UPB.BussinessLogic.Models;

namespace UPB.BussinessLogic.Managers
{
    public class PatientCodeManager
    { 
        private readonly IConfiguration _configuration;

        public PatientCodeManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<PatientModel>> GetPatientsAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(_configuration.GetConnectionString("PatientsAPI"));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<PatientModel>>(responseStream);
                
            }
            catch(Exception ex) 
            {
                FailedToGetDataException fe = new FailedToGetDataException(ex.Message);
                Log.Error(fe.LogMessage("GetPatientsAsync"));
                Log.Error("StackTrace: " + ex.StackTrace);
                throw fe;
            }
           
            
        }

        

        public async Task<List<string>> GetPatientsCodesAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(_configuration.GetConnectionString("PatientsAPI"));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                List<PatientModel> patients = await JsonSerializer.DeserializeAsync<List<PatientModel>>(responseStream);
                List<string> result = new List<string>();
                foreach(var patient in patients)
                {
                    result.Add($"{patient.name[0]}{patient.lastName[0]}-{patient.ci}");
                }
                return result;

            }
            catch (Exception ex)
            {
                FailedToGetDataException fe = new FailedToGetDataException(ex.Message);
                Log.Error(fe.LogMessage("GetPatientsAsync"));
                Log.Error("StackTrace: " + ex.StackTrace);
                throw fe;
            }
        }

        // Gets the code from a backing service api and then rewrites de info
        public async Task<string> GetPatientCodeAsync(string ci)
        {
            Log.Information($"CI: {ci}");
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{_configuration.GetConnectionString("PatientsAPI")}/{ci}");
                if(response.IsSuccessStatusCode) 
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();

                    PatientModel patient = await JsonSerializer.DeserializeAsync<PatientModel>(responseStream);
                    string result = $"{patient.name[0]}{patient.lastName[0]}-{patient.ci}";
                    return result;
                }
                else
                {
                    NonFoundPatientException nonFoundEx = new NonFoundPatientException();
                    Log.Error($"The Patient with the CI: {ci} was not found");
                    throw nonFoundEx;
                }
                

            }
            catch (Exception ex)
            {
                FailedToGetDataException fe = new FailedToGetDataException(ex.Message);
                Log.Error(fe.LogMessage("GetPatientsAsync"));
                Log.Error("StackTrace: " + ex.StackTrace);
                throw fe;
            }
        }
    }
}
