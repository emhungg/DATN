using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudySystem.Data.EF.Seed_Data.ClassMap;
using StudySystem.Data.Entites;
using StudySystem.Infrastructure.CommonConstant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Seed_Data
{
    public class DbInit
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DbInit> _logger;
        public DbInit(AppDbContext context, ILogger<DbInit> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Seed()
        {
            GeneratorFile generatorFile = new GeneratorFile();
            var excutionStrategy = _context.Database.CreateExecutionStrategy();
            await excutionStrategy.Execute(async () =>
            {
                using (var db = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        //if (!_context.UserDetails.Any())
                        //{
                        //    List<UserDetail> userDetails = generatorFile.CsvDataGenerator<UserDetail, UserDetailMap>(CommonConstant.CsvFileUserDetails);
                        //    await _context.BulkInsertAsync(userDetails).ConfigureAwait(false);
                        //}

                        if (!_context.AdministrativeRegions.Any())
                        {
                            List<AdministrativeRegion> administrativeRegions = generatorFile.CsvDataGenerator<AdministrativeRegion, AdministrativeRegonMap>(CommonConstant.CsvAdministrativeRegions);
                            await _context.BulkInsertAsync(administrativeRegions).ConfigureAwait(false);
                        }

                        if (!_context.AdministrativeUnits.Any())
                        {
                            List<AdministrativeUnit> administrativeUnits = generatorFile.CsvDataGenerator<AdministrativeUnit, AdministrativeUnitMap>(CommonConstant.CsvAdministrativeUnits);
                            await _context.BulkInsertAsync(administrativeUnits).ConfigureAwait(false);
                        }

                        if (!_context.Provinces.Any())
                        {
                            List<Province> provinces = generatorFile.CsvDataGenerator<Province, ProvinceMap>(CommonConstant.CsvProvinces);
                            await _context.BulkInsertAsync(provinces).ConfigureAwait(false);
                        }

                        if (!_context.Districts.Any())
                        {
                            List<District> districts = generatorFile.CsvDataGenerator<District, DistrictMap>(CommonConstant.CsvDistricts);
                            await _context.BulkInsertAsync(districts).ConfigureAwait(false);
                        }

                        if (!_context.Wards.Any())
                        {
                            List<Ward> wards = generatorFile.CsvDataGenerator<Ward, WardMap>(CommonConstant.CsvWards);
                            await _context.BulkInsertAsync(wards).ConfigureAwait(false);
                        }

                        //if (!_context.Categories.Any())
                        //{
                        //    List<Category> categories = generatorFile.CsvDataGenerator<Category, CategoryMap>(CommonConstant.CsvCategories);
                        //    await _context.BulkInsertAsync(categories).ConfigureAwait(false);
                        //}

                        await db.CommitAsync();
                        _logger.LogInformation("Init Complement to db");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        await db.RollbackAsync();
                    }
                }
            });

        }
    }
}
