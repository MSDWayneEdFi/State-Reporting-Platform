﻿/// <reference path="../Report-Base/report-base.module.ts" />

module App.Reports.AttendanceTrends {
    import LineChartModel = Models.LineChartModel;

    class AttendanceTrendsReportView extends ReportBaseView {
        resolve = {
            report: ['englishLanguageLearnerStatuses', 'ethnicities', 'grades',
                'lunchStatuses', 'specialEducationStatuses', 'schoolYears', (
                    englishLanguageLearnerStatuses: string[], ethnicities: string[], grades: string[],
                    lunchStatuses: string[], specialEducationStatuses: string[], schoolYears: Models.FilterValueModel[]
                ) => {
                    var filters = [
                        new Models.FilterModel<number>(schoolYears, 'School Years', 'SchoolYears', true),
                        new Models.FilterModel<number>(grades, 'Grade Levels', 'Grades', true),
                        new Models.FilterModel<number>(ethnicities, 'Ethnicities', 'Ethnicities', true),
                        new Models.FilterModel<number>(lunchStatuses, 'Meal Plans', 'LunchStatuses', true),
                        new Models.FilterModel<number>(specialEducationStatuses, 'Education Types', 'SpecialEducationStatuses', true),
                        new Models.FilterModel<number>(englishLanguageLearnerStatuses, 'Language Learners', 'EnglishLanguageLearnerStatuses', true)
                    ];

                    var charts = [
                        new LineChartModel<number>('attendanceTrends', 'byGrade'),
                        new LineChartModel<number>('attendanceTrends','byEthnicity'),
                        new LineChartModel<number>('attendanceTrends','byLunchStatus'),
                        new LineChartModel<number>('attendanceTrends','bySpecialEducation'),
                        new LineChartModel<number>('attendanceTrends','byEnglishLanguageLearner')
                    ];

                    return {
                        filters: filters,
                        charts: charts,
                        title: 'Attendance Trends',
                        model: new Models.EnrollmentFilterModel()
                    }
                }],
            englishLanguageLearnerStatuses: ['api', (api: IApi) => {
                return api.enrollmentFilters.getEnglishLanguageLearnerStatuses();
            }],
            ethnicities: ['api', (api: IApi) => {
                return api.enrollmentFilters.getEthnicities();
            }],
            grades: ['api', (api: IApi) => {
                return api.enrollmentFilters.getGrades();
            }],
            lunchStatuses: ['api', (api: IApi) => {
                return api.enrollmentFilters.getLunchStatuses();
            }],
            specialEducationStatuses: ['api', (api: IApi) => {
                return api.enrollmentFilters.getSpecialEducationStatuses();
            }],
            schoolYears: ['api', (api: IApi) => {
                return api.enrollmentFilters.getSchoolYears();
            }]
        }
    }

    class AttendanceTrendsConfig {
        static $inject = ['$stateProvider', 'settings'];

        constructor($stateProvider: ng.ui.IStateProvider, settings: ISystemSettings) {

            $stateProvider.state('app.reports.attendance-trends',
                {
                    url: '/attendance-trends',
                    views: {
                        'report@app.reports': new AttendanceTrendsReportView(settings)
                    }
                });
        }
    }

    angular
        .module('app.reports.attendance-trends', [])
        .config(AttendanceTrendsConfig);
}