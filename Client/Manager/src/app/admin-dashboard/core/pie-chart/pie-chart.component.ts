import { ChartOptions } from 'chart.js';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.scss']
})
export class PieChartComponent {
  @Input() pieChartDatasets: any;
  @Input() labelChart: any;
  @Input() stylePieChart: any;
  public pieChartOptions: ChartOptions<'pie'> = {
    responsive: false,
  };

  public pieChartLegend = true;
  public pieChartPlugins = [];

}
