import { Component } from '@angular/core';
import { NewsService } from '../news.service';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-news-details',
  templateUrl: './news-details.component.html',
  styleUrls: ['./news-details.component.css']
})
export class NewsDetailsComponent {

  newsDetail: any;
  htmlContent!: any;
  /**
   *
   */
  constructor(private service: NewsService, private route: ActivatedRoute,private sanitizer: DomSanitizer) {
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.route.paramMap.subscribe(params => {
      const id = this.route.snapshot.paramMap.get('id') as string;
      this.service.getNewsById(Number(id)).subscribe(res => {
        this.newsDetail = res
        this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(res.content);
      })
    })
  }
}
