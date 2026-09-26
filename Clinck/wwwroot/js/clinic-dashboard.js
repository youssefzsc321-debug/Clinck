var chart = null;

$(document).ready(function () {
    // تشغيل الرسوم البيانية عند تحميل الصفحة
    drawAppointmentsChart();
    drawDepartmentsChart();
});

function drawAppointmentsChart(startDate = null, endDate = null) {
    var element = document.getElementById('AppointmentsPerDay');
    if (!element) return;

    if (chart) {
        chart.destroy();
        chart = null;
    }

    var height = parseInt(KTUtil.css(element, 'height')) || 350;
    var labelColor = KTUtil.getCssVariableValue('--kt-gray-500');
    var borderColor = KTUtil.getCssVariableValue('--kt-gray-200');
    var baseColor = KTUtil.getCssVariableValue('--kt-primary');
    var lightColor = KTUtil.getCssVariableValue('--kt-primary-light');

    var url = '/Dashboard/GetAppointmentsPerDay';
    if (startDate && endDate) {
        url += `?startDate=${encodeURIComponent(startDate)}&endDate=${encodeURIComponent(endDate)}`;
    }

    $.get({
        url: url,
        success: function (data) {
            var values = data.map(item => Number(item.value));
            var categories = data.map(item => item.label);
            var maxValue = Math.max(...values, 1);

            var options = {
                series: [{ name: 'Appointments', data: values }],
                chart: {
                    fontFamily: 'inherit',
                    type: 'area',
                    height: height,
                    toolbar: { show: false }
                },
                legend: { show: false },
                dataLabels: { enabled: false },
                fill: { type: 'solid', opacity: 1 },
                stroke: { curve: 'smooth', show: true, width: 3, colors: [baseColor] },
                xaxis: {
                    categories: categories,
                    axisBorder: { show: false },
                    axisTicks: { show: false },
                    labels: { style: { colors: labelColor, fontSize: '12px' } }
                },
                yaxis: {
                    min: 0,
                    tickAmount: maxValue > 5 ? 5 : maxValue,
                    labels: { style: { colors: labelColor, fontSize: '12px' } }
                },
                colors: [lightColor],
                grid: {
                    borderColor: borderColor,
                    strokeDashArray: 4,
                    yaxis: { lines: { show: true } }
                },
                markers: { strokeColor: baseColor, strokeWidth: 3 }
            };

            chart = new ApexCharts(element, options);
            chart.render();
        }
    });
}

function drawDepartmentsChart() {
    var canvas = document.getElementById('PatientsPerDepartment');
    if (!canvas) return;

    $.get({
        url: '/Dashboard/GetPatientsPerDepartment',
        success: function (data) {
            var labels = data.map(item => item.departmentName);
            var values = data.map(item => Number(item.count));

            var chartData = {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: [
                        KTUtil.getCssVariableValue('--kt-primary'),
                        KTUtil.getCssVariableValue('--kt-success'),
                        KTUtil.getCssVariableValue('--kt-warning'),
                        KTUtil.getCssVariableValue('--kt-danger'),
                        KTUtil.getCssVariableValue('--kt-info')
                    ],
                    borderRadius: 8
                }]
            };

            var config = {
                type: 'doughnut',
                data: chartData,
                options: {
                    responsive: true,
                    plugins: {
                        legend: { position: 'bottom' },
                        title: { display: false }
                    }
                }
            };

            new Chart(canvas, config);
        }
    });
}