import { ChartConfiguration } from 'chart.js';
import { ChartOptions } from 'chart.js';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.scss']
})
export class LineChartComponent {
  @Input() witdChart!: any;
  @Input() heightChart!: any;
  @Input() lineChartData: any;
  public lineChartOptions: ChartOptions<'line'> = {
    responsive: false
  };
  public lineChartLegend = true;
}
