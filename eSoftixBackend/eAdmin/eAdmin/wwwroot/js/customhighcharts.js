function DonutChart(data) {

    Highcharts.chart('container', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        exporting: {
            buttons: {
                contextButton: {
                    enabled: false
                }
            }
        },
        credits: {
            enabled: false
        },
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 600
                }
            }]
        },
        title: {
            text: data.chart_title,
            y: 225
        },
        legend: {
            enabled: data.show_legend
        },
        tooltip: {
            pointFormat: data.tooltip
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    formatter: function () {
                        return this.key + ': ' + this.y ;
                    }
                },
                showInLegend: true
            }
        },
        series: data.series
    });


}