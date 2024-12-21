import { ChartConfiguration } from 'chart.js';
import { Component } from '@angular/core';
import { ChartsService } from 'src/app/service/charts.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  userOnlineLabel = ['Người đăng ký onl', 'Người mua hàng'];
  topSellerlabel: string[] = [];
  topSellerData: any = [];
  leastSellerlabel: string[] = [];
  leastSellerData: any = [];
  monthVal: number = 0;
  monthData = [{
    "month": "0"
  }, {
    "month": "1"
  },
  {
    "month": "2"
  },
  {
    "month": "3"
  },
  {
    "month": "4"
  },
  {
    "month": "5"
  },
  {
    "month": "6"
  },
  {
    "month": "7"
  },
  {
    "month": "8"
  },
  {
    "month": "9"
  },
  {
    "month": "10"
  },
  {
    "month": "11"
  },
  {
    "month": "12"
  }]

  constructor(private chart: ChartsService) {

  }
  dataAPI: any;
  interval: any;
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.chart.getDataToShow(this.monthVal).subscribe(res => {
      this.dataAPI = res;
      // xử lý chart doanh thu
      this.lineChartData = {
        labels: [
          'January',
          'February',
          'March',
          'April',
          'May',
          'June',
          'July',
          'August',
          'September',
          'Octember',
          'November',
          'December'
        ],
        datasets: [
          {
            data: res.revenusByMonth,
            label: 'Doanh thu các tháng trong năm ' + this.thisYear + '(VND)',
            fill: true,
            tension: 0.5,
            borderColor: 'black',
            backgroundColor: 'rgba(255,0,0,0.3)'
          }
        ]
      };

      // compare best seller and least seller
      this.topSellerlabel = [res.compareBestSellingData.nameProductFirst, res.compareBestSellingData.nameProductSecond, res.compareBestSellingData.nameProductLast];
      this.topSellerData = [{
        data: [res.compareBestSellingData.dataProductFirst, res.compareBestSellingData.dataProductSecond, res.compareBestSellingData.dataProductLast]
      }];

      this.leastSellerlabel = [res.leastSoldData.nameProductFirst, res.leastSoldData.nameProductSecond, res.leastSoldData.nameProductLast];
      this.leastSellerData = [{
        data: [res.leastSoldData.dataProductFirst, res.leastSoldData.dataProductSecond, res.leastSoldData.dataProductLast]
      }];

      // xử lý chart pie
      this.pieChartDatasets = [{
        data: res.overviewCustomer
      }]
    });
    this.interval = setInterval(() => {
      this.chart.getDataToShow(this.monthVal).subscribe(res => {
        if (!this.isEqual(this.dataAPI, res)) {
          this.dataAPI = res
          if (this.monthVal == 0) {
            // xử lý chart doanh thu
            this.lineChartData = {
              labels: [
                'January',
                'February',
                'March',
                'April',
                'May',
                'June',
                'July',
                'August',
                'September',
                'Octember',
                'November',
                'December'
              ],
              datasets: [
                {
                  data: res.revenusByMonth,
                  label: 'Doanh thu các tháng trong năm ' + this.thisYear + '(VND)',
                  fill: true,
                  tension: 0.5,
                  borderColor: 'black',
                  backgroundColor: 'rgba(255,0,0,0.3)'
                }
              ]
            };
          } else {
            this.lineChartData = {
              labels: this.generateDaysInMonth(this.monthVal, this.thisYear),
              datasets: [
                {
                  data: res.revenusByMonth,
                  label: 'Doanh thu của tháng ' + this.monthVal + ' năm ' + this.thisYear + '(VND)',
                  fill: true,
                  tension: 0.5,
                  borderColor: 'black',
                  backgroundColor: 'rgba(255,0,0,0.3)'
                }
              ]
            };
          }
          // 3 sản phẩm bán chạy nhất
          this.topSellerlabel = [res.compareBestSellingData.nameProductFirst, res.compareBestSellingData.nameProductSecond, res.compareBestSellingData.nameProductLast];
          this.topSellerData = [{
            data: [res.compareBestSellingData.dataProductFirst, res.compareBestSellingData.dataProductSecond, res.compareBestSellingData.dataProductLast]
          }];
          // 3 sản phẩm bán ít nhất
          this.leastSellerlabel = [res.leastSoldData.nameProductFirst, res.leastSoldData.nameProductSecond, res.leastSoldData.nameProductLast];
          this.leastSellerData = [{
            data: [res.leastSoldData.dataProductFirst, res.leastSoldData.dataProductSecond, res.leastSoldData.dataProductLast]
          }];
          // thông tin người dùng đăng ký
          this.pieChartDatasets = [{
            data: res.overviewCustomer
          }]
          console.log('update')
        }

      });
      console.log("get-data")
    }, 5000)
  }

  pieChartDatasets: any;
  thisYear: number = new Date().getUTCFullYear();
  //#region  chart doanh thu
  widthChart = '750rem';
  heightChart = '400rem';
  public lineChartData!: ChartConfiguration<'line'>['data'];
  //#endregion

  //#region  chart total
  widthChartTotal = "1260rem"
  heightChartTotal = "500rem"
  public lineChartDataTotal!: ChartConfiguration<'line'>['data'];
  //#endregion


  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    if (this.interval) {
      clearInterval(this.interval)
      console.log('stop-call')
    }
  }

  // compare
  isEqual(obj1: any, obj2: any): boolean {
    return JSON.stringify(obj1) === JSON.stringify(obj2);
  }

  // hàm check thay đổi 
  changeStatasticToTal(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.monthVal = parseInt(selectedValue);
    this.chart.getDataToShow(this.monthVal).subscribe(res => {
      this.dataAPI = res;
      if (this.monthVal == 0) {
        // xử lý chart doanh thu
        this.lineChartData = {
          labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'Octember',
            'November',
            'December'
          ],
          datasets: [
            {
              data: res.revenusByMonth,
              label: 'Doanh thu các tháng trong năm ' + this.thisYear + '(VND)',
              fill: true,
              tension: 0.5,
              borderColor: 'black',
              backgroundColor: 'rgba(255,0,0,0.3)'
            }
          ]
        };
      } else {
        this.lineChartData = {
          labels: this.generateDaysInMonth(this.monthVal, this.thisYear),
          datasets: [
            {
              data: res.revenusByMonth,
              label: 'Doanh thu của tháng ' + this.monthVal + ' năm ' + this.thisYear + '(VND)',
              fill: true,
              tension: 0.5,
              borderColor: 'black',
              backgroundColor: 'rgba(255,0,0,0.3)'
            }
          ]
        };
      }
    });
  }

  generateDaysInMonth(month: number, year: number): Array<number> {
    // Lấy số ngày trong tháng
    const date = new Date(year, month, 0);
    const daysInMonth = date.getDate();
    // Tạo list từ 1 đến số ngày trong tháng
    return Array.from({ length: daysInMonth }, (_, i) => i + 1);
  }
}

