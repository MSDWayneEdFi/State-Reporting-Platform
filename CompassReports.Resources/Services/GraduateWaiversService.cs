﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompassReports.Data;
using CompassReports.Data.Entities;
using CompassReports.Resources.Models;

namespace CompassReports.Resources.Services
{
    public interface IGraduateWaiversService
    {
        PieChartModel<int> Get(GraduateFilterModel model);
        PercentageTotalBarChartModel ByEnglishLanguageLearner(GraduateFilterModel model);
        PercentageTotalBarChartModel ByEthnicity(GraduateFilterModel model);
        PercentageTotalBarChartModel ByLunchStatus(GraduateFilterModel model);
        PercentageTotalBarChartModel BySpecialEducation(GraduateFilterModel model);
    }

    public class GraduateWaiversService : IGraduateWaiversService
    {
        private readonly IRepository<GraduationFact> _graduationFactRepository;

        public GraduateWaiversService(IRepository<GraduationFact> graduationFactRepository)
        {
            _graduationFactRepository = graduationFactRepository;
        }

        public PieChartModel<int> Get(GraduateFilterModel model)
        {
            var query = BaseQuery(model);

            var results = query.GroupBy(x => x.GraduationStatus.GraduationWaiver)
                .Select(x => new
                {
                    GraduationWaiver = x.Key,
                    Total = x.Sum(y => y.GraduationStudentCount)
                }).OrderBy(x => x.GraduationWaiver)
                .ToList();

            var total = results.Sum(x => x.Total);

            return new PieChartModel<int>
            {
                Title = "Graduation Waiver Status",
                TotalRowTitle = "Waiver Total",
                Headers = new List<string> { "", "Graduation Waiver", "Status Count" },
                PercentageHeaders = new List<string> { "", "Graduation Waiver", "Status Percentage" },
                Labels = results.Select(x => x.GraduationWaiver).ToList(),
                Data = results.Select(x => x.Total).ToList(),
                Percentages = results.Select(x => GetPercentage(x.Total, total)).ToList(),
                ShowChart = true,
                Total = total
            };
        }

        public PercentageTotalBarChartModel ByEnglishLanguageLearner(GraduateFilterModel model)
        {
            var query = BaseQuery(model).ToList();

            var results = query.GroupBy(x => new { x.GraduationStatus.GraduationWaiver, Property = x.Demographic.EnglishLanguageLearnerStatus })
                .Select(x => new
                {
                    GraduationWaiver = x.Key.GraduationWaiver,
                    Property = x.Key.Property,
                    Total = x.Sum(y => y.GraduationStudentCount)
                }).ToList();

            var graduationWaivers = results.Select(x => x.GraduationWaiver).Distinct().OrderBy(x => x).ToList();

            var headers = new List<string> { "", "Language Statuses" };
            headers.AddRange(graduationWaivers);

            var propertyValues = results.Select(x => x.Property).Distinct().OrderBy(x => x).ToList();

            var data = new List<List<PercentageTotalDataModel>>();
            foreach (var value in propertyValues)
            {
                var values = new List<PercentageTotalDataModel>();
                var propertyTotal = results.Where(x => x.Property == value).Sum(x => x.Total);
                var properties = results.Where(x => x.Property == value).ToList();

                foreach (var graduationWaiver in graduationWaivers)
                {
                    var row = properties.FirstOrDefault(x => x.GraduationWaiver == graduationWaiver);
                    var rowTotal = row == null ? 0 : row.Total;
                    values.Add(new PercentageTotalDataModel
                    {
                        Percentage = rowTotal == 0 ? 0 : GetPercentage(rowTotal, propertyTotal),
                        Total = rowTotal
                    });
                }
                data.Add(values);
            }

            var total = results.Sum(x => x.Total);
            var totals = results.GroupBy(x => x.GraduationWaiver)
                .OrderBy(x => x.Key)
                .Select(x => new PercentageTotalDataModel
                {
                    Percentage = GetPercentage(x.Sum(y => y.Total), total),
                    Total = x.Sum(y => y.Total)
                }).ToList();

            return new PercentageTotalBarChartModel
            {
                Title = "Graduation Waiver by English Language Learners",
                Headers = headers,
                Labels = graduationWaivers,
                Series = propertyValues.Select(x => x.ToString()).ToList(),
                Data = data,
                ShowChart = true,
                ShowPercentage = true,
                TotalRowTitle = "Graduation Waiver",
                Totals = totals
            };
        }

        public PercentageTotalBarChartModel ByEthnicity(GraduateFilterModel model)
        {
            var query = BaseQuery(model).ToList();

            var results = query.GroupBy(x => new { x.GraduationStatus.GraduationWaiver, Property = x.Demographic.Ethnicity })
                .Select(x => new
                {
                    GraduationWaiver = x.Key.GraduationWaiver,
                    Property = x.Key.Property,
                    Total = x.Sum(y => y.GraduationStudentCount)
                }).ToList();

            var graduationWaivers = results.Select(x => x.GraduationWaiver).Distinct().OrderBy(x => x).ToList();

            var headers = new List<string> { "", "Ethnicities" };
            headers.AddRange(graduationWaivers);

            var propertyValues = results.Select(x => x.Property).Distinct().OrderBy(x => x).ToList();

            var data = new List<List<PercentageTotalDataModel>>();
            foreach (var value in propertyValues)
            {
                var values = new List<PercentageTotalDataModel>();
                var propertyTotal = results.Where(x => x.Property == value).Sum(x => x.Total);
                var properties = results.Where(x => x.Property == value).ToList();

                foreach (var waiver in graduationWaivers)
                {
                    var row = properties.FirstOrDefault(x => x.GraduationWaiver == waiver);
                    var rowTotal = row == null ? 0 : row.Total;
                    values.Add(new PercentageTotalDataModel
                    {
                        Percentage = rowTotal == 0 ? 0 : GetPercentage(rowTotal, propertyTotal),
                        Total = rowTotal
                    });
                }
                data.Add(values);
            }

            var total = results.Sum(x => x.Total);
            var totals = results.GroupBy(x => x.GraduationWaiver)
                .OrderBy(x => x.Key)
                .Select(x => new PercentageTotalDataModel
                {
                    Percentage = GetPercentage(x.Sum(y => y.Total), total),
                    Total = x.Sum(y => y.Total)
                }).ToList();

            return new PercentageTotalBarChartModel
            {
                Title = "Graduation Waiver by Ethnicity",
                Headers = headers,
                Labels = graduationWaivers,
                Series = propertyValues.Select(x => x.ToString()).ToList(),
                Data = data,
                ShowChart = true,
                ShowPercentage = true,
                TotalRowTitle = "Graduation Waiver",
                Totals = totals
            };
        }

        public PercentageTotalBarChartModel ByLunchStatus(GraduateFilterModel model)
        {
            var query = BaseQuery(model).ToList();

            var results = query.GroupBy(x => new { x.GraduationStatus.GraduationWaiver, Property = x.Demographic.FreeReducedLunchStatus })
                .Select(x => new
                {
                    GraduationWaiver = x.Key.GraduationWaiver,
                    Property = x.Key.Property,
                    Total = x.Sum(y => y.GraduationStudentCount)
                }).ToList();

            var graduationWaivers = results.Select(x => x.GraduationWaiver).Distinct().OrderBy(x => x).ToList();

            var headers = new List<string> { "", "Lunch Statuses" };
            headers.AddRange(graduationWaivers);

            var propertyValues = results.Select(x => x.Property).Distinct().OrderBy(x => x).ToList();

            var data = new List<List<PercentageTotalDataModel>>();
            foreach (var value in propertyValues)
            {
                var values = new List<PercentageTotalDataModel>();
                var propertyTotal = results.Where(x => x.Property == value).Sum(x => x.Total);
                var properties = results.Where(x => x.Property == value).ToList();

                foreach (var graduationWaiver in graduationWaivers)
                {
                    var row = properties.FirstOrDefault(x => x.GraduationWaiver == graduationWaiver);
                    var rowTotal = row == null ? 0 : row.Total;
                    values.Add(new PercentageTotalDataModel
                    {
                        Percentage = rowTotal == 0 ? 0 : GetPercentage(rowTotal, propertyTotal),
                        Total = rowTotal
                    });
                }
                data.Add(values);
            }

            var total = results.Sum(x => x.Total);
            var totals = results.GroupBy(x => x.GraduationWaiver)
                .OrderBy(x => x.Key)
                .Select(x => new PercentageTotalDataModel
                {
                    Percentage = GetPercentage(x.Sum(y => y.Total), total),
                    Total = x.Sum(y => y.Total)
                }).ToList();

            return new PercentageTotalBarChartModel
            {
                Title = "Graduation Waiver by Free/Reduced Price Meals",
                Headers = headers,
                Labels = graduationWaivers,
                Series = propertyValues.Select(x => x.ToString()).ToList(),
                Data = data,
                ShowChart = true,
                ShowPercentage = true,
                TotalRowTitle = "Graduation Waiver",
                Totals = totals
            };
        }

        public PercentageTotalBarChartModel BySpecialEducation(GraduateFilterModel model)
        {
            var query = BaseQuery(model).ToList();

            var results = query.GroupBy(x => new { x.GraduationStatus.GraduationWaiver, Property = x.Demographic.SpecialEducationStatus })
                .Select(x => new
                {
                    GraduationWaiver = x.Key.GraduationWaiver,
                    Property = x.Key.Property,
                    Total = x.Sum(y => y.GraduationStudentCount)
                }).ToList();

            var graduationWaivers = results.Select(x => x.GraduationWaiver).Distinct().OrderBy(x => x).ToList();

            var headers = new List<string> { "", "Education Statuses" };
            headers.AddRange(graduationWaivers);

            var propertyValues = results.Select(x => x.Property).Distinct().OrderBy(x => x).ToList();

            var data = new List<List<PercentageTotalDataModel>>();
            foreach (var value in propertyValues)
            {
                var values = new List<PercentageTotalDataModel>();
                var propertyTotal = results.Where(x => x.Property == value).Sum(x => x.Total);
                var properties = results.Where(x => x.Property == value).ToList();

                foreach (var graduationWaiver in graduationWaivers)
                {
                    var row = properties.FirstOrDefault(x => x.GraduationWaiver == graduationWaiver);
                    var rowTotal = row == null ? 0 : row.Total;
                    values.Add(new PercentageTotalDataModel
                    {
                        Percentage = rowTotal == 0 ? 0 : GetPercentage(rowTotal, propertyTotal),
                        Total = rowTotal
                    });
                }
                data.Add(values);
            }

            var total = results.Sum(x => x.Total);
            var totals = results.GroupBy(x => x.GraduationWaiver)
                .OrderBy(x => x.Key)
                .Select(x => new PercentageTotalDataModel
                {
                    Percentage = GetPercentage(x.Sum(y => y.Total), total),
                    Total = x.Sum(y => y.Total)
                }).ToList();

            return new PercentageTotalBarChartModel
            {
                Title = "Graduation Waiver by Special Education",
                Headers = headers,
                Labels = graduationWaivers,
                Series = propertyValues.Select(x => x.ToString()).ToList(),
                Data = data,
                ShowChart = true,
                ShowPercentage = true,
                TotalRowTitle = "Graduation Waiver",
                Totals = totals
            };
        }

        private static double GetPercentage(int subTotal, int total)
        {
            return Math.Round(100 * ((double)subTotal / (double)total), 2);
        }

        private IQueryable<GraduationFact> BaseQuery(GraduateFilterModel model)
        {
            var query = _graduationFactRepository
                .GetAll()
                .Where(x => x.SchoolYearKey == model.CohortYear && 
                    x.Demographic.ExpectedGraduationYear == model.ExpectedGraduationYear.ToString() &&
                    x.GraduationStatus.GraduationWaiver != "Not Applicable");

            if (model.EnglishLanguageLearnerStatuses != null && model.EnglishLanguageLearnerStatuses.Any())
                query = query.Where(x => model.EnglishLanguageLearnerStatuses.Contains(x.Demographic.EnglishLanguageLearnerStatus));

            if (model.Ethnicities != null && model.Ethnicities.Any())
                query = query.Where(x => model.Ethnicities.Contains(x.Demographic.Ethnicity));

            if (model.LunchStatuses != null && model.LunchStatuses.Any())
                query = query.Where(x => model.LunchStatuses.Contains(x.Demographic.FreeReducedLunchStatus));

            if (model.SpecialEducationStatuses != null && model.SpecialEducationStatuses.Any())
                query = query.Where(x => model.SpecialEducationStatuses.Contains(x.Demographic.SpecialEducationStatus));

            return query;
        }
    }
}