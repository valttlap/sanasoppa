import { Component, OnInit } from '@angular/core';
import { GameHubService } from './services/gamehub.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  constructor(private gameHubService: GameHubService) {}

  ngOnInit(): void {
    this.gameHubService.startConnection();
  }
}
