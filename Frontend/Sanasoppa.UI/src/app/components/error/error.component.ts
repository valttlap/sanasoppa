import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
})
export class ErrorComponent implements OnInit {
  errorMessage?: string | null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.errorMessage = this.route.snapshot.paramMap.get('message');
  }
}
