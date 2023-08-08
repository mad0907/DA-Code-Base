using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAWebAPIs.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Net.NetworkInformation;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAWebAPIs.Controllers
{
    [Route("api/[controller]")]
    public class AgencyController : Controller
    {
        private readonly AgencyDBContext _context;
        private readonly Container _container;
        private readonly string _databaseName;
        private readonly string _apiKey;

        public AgencyController(AgencyDBContext context, CosmosClient cosmosClient, List<KeyValuePair<string,string>> keys)
        {
            _context = context;
            _databaseName = keys.FirstOrDefault(k => k.Key == "DBName").Value;
            _apiKey= keys.FirstOrDefault(k => k.Key == "APIKey").Value;
            Database database = cosmosClient.GetDatabase(_databaseName);
            _container = database.GetContainer("agency_data");
        }

        // GET: api/values
        [HttpGet(nameof(GetAllAgencyData))]
        public async Task<ActionResult<IEnumerable<AgencyData>>> GetAllAgencyData()
        {
            try
            {
                string apiKey = HttpContext.Request.Headers["X-SERVICE-KEY"];
                if (apiKey!=_apiKey)
                {
                    return Unauthorized("Invalid Token");
                }
                if (_context.agencyDatas == null)
                {
                    return NotFound();
                }
                return await _context.agencyDatas.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/values/5
        [HttpGet(nameof(GetAgencyData) + "/{id}")]
        public async Task<ActionResult<AgencyData>> GetAgencyData(Guid id)
        {
            try
            {
                string apiKey = HttpContext.Request.Headers["X-SERVICE-KEY"];
                if (apiKey != _apiKey)
                {
                    return Unauthorized("Invalid Token");
                }
                if (_context.agencyDatas == null)
                {
                    return NotFound();
                }
                var proj = await _context.agencyDatas.FirstOrDefaultAsync(p => p.id == id);
                if (proj == null)
                {
                    return NotFound();
                }
                return proj;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/values
        [HttpPost(nameof(UploadBulkList))]
        public async Task<ActionResult> UploadBulkList(IFormFile file)
        {
            try
            {
                string apiKey = HttpContext.Request.Headers["X-SERVICE-KEY"];
                if (apiKey != _apiKey)
                {
                    return Unauthorized("Invalid Token");
                }
                if (file == null || file.Length <= 0)
                {
                    return BadRequest("No file uploaded.");
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(file.OpenReadStream());
                var worksheet = package.Workbook.Worksheets[0];
                var documents = new List<AgencyData>();
                var failedItems = new List<AgencyData>();
                AgencyData jsonDocument;

                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Value?.ToString()))
                    {
                        jsonDocument = new AgencyData();
                        jsonDocument = CreateJsonDocument(worksheet, row);
                        documents.Add(jsonDocument);
                    }
                }

                foreach (var document in documents)
                {
                    try
                    {
                        _context.agencyDatas.Add(document);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        failedItems.Add(document);
                    }
                }

                return Ok("Upload completed." + failedItems);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost(nameof(PostAgencyData))]
        public async Task<ActionResult> PostAgencyData([FromBody] AgencyDataDto data)
        {
            try
            {
                string apiKey = HttpContext.Request.Headers["X-SERVICE-KEY"];
                if (apiKey != _apiKey)
                {
                    return Unauthorized("Invalid Token");
                }
                Guid newGuid = Guid.NewGuid();
                var newAgencyData = new AgencyData
                {
                    id = newGuid,
                    PatientName = data.PatientName,
                    StartOfCare = data.StartOfCare,
                    StartOfEpisode = data.StartOfEpisode,
                    EndOfEpisode = data.EndOfEpisode,
                    EpisodeStatus = data.EpisodeStatus,
                    MedicalRecordNo = data.MedicalRecordNo,
                    ServiceLine = data.ServiceLine,
                    PatientAddress = data.PatientAddress,
                    Zip = data.Zip,
                    PayorSource = data.PayorSource,
                    PhysicianNPI = data.PhysicianNPI,
                    PhysicianName = data.PhysicianName,
                    PGEHRId = data.PGEHRId,
                    AgencyType = data.AgencyType,
                    AgencyEHRId = data.AgencyEHRId,
                    BillingProvider = data.BillingProvider,
                    NPI = data.NPI,
                    FirstDiagnosis = data.FifthDiagnosis,
                    SecondDiagnosis = data.SecondDiagnosis,
                    ThirdDiagnosis = data.ThirdDiagnosis,
                    FourthDiagnosis = data.FourthDiagnosis,
                    FifthDiagnosis = data.FifthDiagnosis,
                    SixthDiagnosis = data.SixthDiagnosis,
                    Line1DOSFrom = data.Line1DOSFrom,
                    Line1DOSTo = data.Line1DOSTo,
                    Line1POS = data.Line1POS,
                    SupervisingProvider = data.SupervisingProvider,
                    PhysicianPhone = data.PhysicianPhone,
                    PhysicianAddress = data.PhysicianAddress,
                    CityStateZip = data.CityStateZip,
                    NumberOfEpisodes = data.NumberOfEpisodes,
                    Status485 = data.Status485,
                    F2fStatus = data.F2fStatus,
                    NumberOfDocuments = data.NumberOfDocuments,
                    InsuranceCompanyName = data.InsuranceCompanyName,
                    InsuranceID = data.InsuranceID,
                    Agency = data.Agency,
                    PatientAccountNo = data.PatientAccountNo,
                    PatientSex = data.PatientSex,
                    PatientCity = data.PatientCity,
                    PatientState = data.PatientState,
                    PatientZip = data.PatientZip,
                    Line1CPT = data.Line1CPT,
                    Line1Units = data.Line1Units,
                    Line1charges = data.Line1charges,
                    CreatedAt = DateTime.Now.ToShortDateString(),
                    CreatedBy = null

                };
                _context.agencyDatas.Add(newAgencyData);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAgencyData), new { id = newAgencyData.id }, newAgencyData);
            }
            catch (Exception ex)
            {
                return BadRequest("Post Failed! " + ex.Message);
            }
        }

        static AgencyData CreateJsonDocument(ExcelWorksheet worksheet, int row)
        {
            // Create a JSON document from the Excel data
            // You need to implement this method based on your Excel columns and JSON structure
            // Example:
            var jsonDocument = new AgencyData
            {
                id = Guid.NewGuid(),
                PatientName = worksheet.Cells[row, 1].Value?.ToString(),
                StartOfCare = worksheet.Cells[row, 2].Value?.ToString(),
                StartOfEpisode = worksheet.Cells[row, 3].Value?.ToString(),
                EndOfEpisode = worksheet.Cells[row, 4].Value?.ToString(),
                EpisodeStatus = worksheet.Cells[row, 5].Value?.ToString(),
                MedicalRecordNo = worksheet.Cells[row, 6].Value?.ToString(),
                ServiceLine = worksheet.Cells[row, 7].Value?.ToString(),
                PatientAddress = worksheet.Cells[row, 8].Value?.ToString(),
                Zip = worksheet.Cells[row, 9].Value?.ToString(),
                PayorSource = worksheet.Cells[row, 10].Value?.ToString(),
                PhysicianNPI = worksheet.Cells[row, 11].Value?.ToString(),
                PhysicianName = worksheet.Cells[row, 12].Value?.ToString(),
                PGEHRId = worksheet.Cells[row, 13].Value?.ToString(),
                AgencyType = worksheet.Cells[row, 14].Value?.ToString(),
                AgencyEHRId = worksheet.Cells[row, 15].Value?.ToString(),
                BillingProvider = worksheet.Cells[row, 16].Value?.ToString(),
                NPI = worksheet.Cells[row, 17].Value?.ToString(),
                FirstDiagnosis = worksheet.Cells[row, 18].Value?.ToString(),
                SecondDiagnosis = worksheet.Cells[row, 19].Value?.ToString(),
                ThirdDiagnosis = worksheet.Cells[row, 20].Value?.ToString(),
                FourthDiagnosis = worksheet.Cells[row, 21].Value?.ToString(),
                FifthDiagnosis = worksheet.Cells[row, 22].Value?.ToString(),
                SixthDiagnosis = worksheet.Cells[row, 23].Value?.ToString(),
                Line1DOSFrom = worksheet.Cells[row, 24].Value?.ToString(),
                Line1DOSTo = worksheet.Cells[row, 25].Value?.ToString(),
                Line1POS = worksheet.Cells[row, 26].Value?.ToString(),
                SupervisingProvider = worksheet.Cells[row, 27].Value?.ToString(),
                PhysicianPhone = worksheet.Cells[row, 28].Value?.ToString(),
                PhysicianAddress = worksheet.Cells[row, 29].Value?.ToString(),
                CityStateZip = worksheet.Cells[row, 30].Value?.ToString(),
                NumberOfEpisodes = worksheet.Cells[row, 31].Value?.ToString(),
                Status485 = worksheet.Cells[row, 32].Value?.ToString(),
                F2fStatus = worksheet.Cells[row, 33].Value?.ToString(),
                NumberOfDocuments = worksheet.Cells[row, 34].Value?.ToString(),
                InsuranceCompanyName = worksheet.Cells[row, 35].Value?.ToString(),
                InsuranceID = worksheet.Cells[row, 36].Value?.ToString(),
                Agency = worksheet.Cells[row, 37].Value?.ToString(),
                PatientAccountNo = worksheet.Cells[row, 38].Value?.ToString(),
                PatientSex = worksheet.Cells[row, 39].Value?.ToString(),
                PatientCity = worksheet.Cells[row, 40].Value?.ToString(),
                PatientState = worksheet.Cells[row, 41].Value?.ToString(),
                PatientZip = worksheet.Cells[row, 42].Value?.ToString(),
                Line1CPT = worksheet.Cells[row, 43].Value?.ToString(),
                Line1Units = worksheet.Cells[row, 44].Value?.ToString(),
                Line1charges = worksheet.Cells[row, 45].Value?.ToString(),
                CreatedAt = DateTime.Now.ToShortDateString(),
                CreatedBy = null
            };

            return jsonDocument;
        }

        static async Task UploadToCosmosDB(Container container, AgencyData jsonDocument)
        {
            await container.CreateItemAsync(jsonDocument);
        }

        // PUT api/values/5
        [HttpPut(nameof(UpdateAgencyData) + "/{id}")]
        public async Task<IActionResult> UpdateAgencyData(Guid id, [FromBody] AgencyData data)
        {
            try
            {
                string apiKey = HttpContext.Request.Headers["X-SERVICE-KEY"];
                if (apiKey != _apiKey)
                {
                    return Unauthorized("Invalid Token");
                }
                var existingProj = await _context.agencyDatas.FirstOrDefaultAsync(p => p.id == id);
                if (existingProj == null)
                {
                    return NotFound("Record does not exist!");
                }

                if (data.PatientName != null) existingProj.PatientName = data.PatientName;
                if (data.StartOfCare != null) existingProj.StartOfCare = data.StartOfCare;
                if (data.StartOfEpisode != null) existingProj.StartOfEpisode = data.StartOfEpisode;
                if (data.EndOfEpisode != null) existingProj.EndOfEpisode = data.EndOfEpisode;
                if (data.EpisodeStatus != null) existingProj.EpisodeStatus = data.EpisodeStatus;
                if (data.MedicalRecordNo != null) existingProj.MedicalRecordNo = data.MedicalRecordNo;
                if (data.ServiceLine != null) existingProj.ServiceLine = data.ServiceLine;
                if (data.PatientAddress != null) existingProj.PatientAddress = data.PatientAddress;
                if (data.Zip != null) existingProj.Zip = data.Zip;
                if (data.PayorSource != null) existingProj.PayorSource = data.PayorSource;
                if (data.PhysicianNPI != null) existingProj.PhysicianNPI = data.PhysicianNPI;
                if (data.PhysicianName != null) existingProj.PhysicianName = data.PhysicianName;
                if (data.PGEHRId != null) existingProj.PGEHRId = data.PGEHRId;
                if (data.AgencyType != null) existingProj.AgencyType = data.AgencyType;
                if (data.AgencyEHRId != null) existingProj.AgencyEHRId = data.AgencyEHRId;
                if (data.BillingProvider != null) existingProj.BillingProvider = data.BillingProvider;
                if (data.NPI != null) existingProj.NPI = data.NPI;
                if (data.FirstDiagnosis != null) existingProj.FirstDiagnosis = data.FifthDiagnosis;
                if (data.SecondDiagnosis != null) existingProj.SecondDiagnosis = data.SecondDiagnosis;
                if (data.ThirdDiagnosis != null) existingProj.ThirdDiagnosis = data.ThirdDiagnosis;
                if (data.FourthDiagnosis != null) existingProj.FourthDiagnosis = data.FourthDiagnosis;
                if (data.FifthDiagnosis != null) existingProj.FifthDiagnosis = data.FifthDiagnosis;
                if (data.SixthDiagnosis != null) existingProj.SixthDiagnosis = data.SixthDiagnosis;
                if (data.Line1DOSFrom != null) existingProj.Line1DOSFrom = data.Line1DOSFrom;
                if (data.Line1DOSTo != null) existingProj.Line1DOSTo = data.Line1DOSTo;
                if (data.Line1POS != null) existingProj.Line1POS = data.Line1POS;
                if (data.SupervisingProvider != null) existingProj.SupervisingProvider = data.SupervisingProvider;
                if (data.PhysicianPhone != null) existingProj.PhysicianPhone = data.PhysicianPhone;
                if (data.PhysicianAddress != null) existingProj.PhysicianAddress = data.PhysicianAddress;
                if (data.CityStateZip != null) existingProj.CityStateZip = data.CityStateZip;
                if (data.NumberOfEpisodes != null) existingProj.NumberOfEpisodes = data.NumberOfEpisodes;
                if (data.Status485 != null) existingProj.Status485 = data.Status485;
                if (data.F2fStatus != null) existingProj.F2fStatus = data.F2fStatus;
                if (data.NumberOfDocuments != null) existingProj.NumberOfDocuments = data.NumberOfDocuments;
                if (data.InsuranceCompanyName != null) existingProj.InsuranceCompanyName = data.InsuranceCompanyName;
                if (data.InsuranceID != null) existingProj.InsuranceID = data.InsuranceID;
                if (data.Agency != null) existingProj.Agency = data.Agency;
                if (data.PatientAccountNo != null) existingProj.PatientAccountNo = data.PatientAccountNo;
                if (data.PatientSex != null) existingProj.PatientSex = data.PatientSex;
                if (data.PatientCity != null) existingProj.PatientCity = data.PatientCity;
                if (data.PatientState != null) existingProj.PatientState = data.PatientState;
                if (data.PatientZip != null) existingProj.PatientZip = data.PatientZip;
                if (data.Line1CPT != null) existingProj.Line1CPT = data.Line1CPT;
                if (data.Line1Units != null) existingProj.Line1Units = data.Line1Units;
                if (data.Line1charges != null) existingProj.Line1charges = data.Line1charges;

                // Save the changes to the database
                _context.agencyDatas.Update(existingProj);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAgencyData), new { id = existingProj.id }, existingProj);
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed! " + ex.Message);

            }
        }

        // DELETE api/values/5
        [HttpDelete(nameof(DeleteAgencyData) + "/{id}")]
        public async Task<ActionResult> DeleteAgencyData(Guid id)
        {
            try
            {
                string apiKey = HttpContext.Request.Headers["X-SERVICE-KEY"];
                if (apiKey != _apiKey)
                {
                    return Unauthorized("Invalid Token");
                }
                var existingProj = await _context.agencyDatas.FirstOrDefaultAsync(p => p.id == id);
                if (existingProj != null)
                {
                    _context.Remove(existingProj);
                    await _context.SaveChangesAsync();
                    return Ok("Record deleted!");

                }
                else
                {
                    return NotFound("Record not found!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Project Could not be deleted! " + ex.Message);
            }
        }



    }
}

