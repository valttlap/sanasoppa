import { Injectable } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';
import { environment as env } from '../../environments/environment';
import { BusyService } from './busy.service';
import { HttpClient } from '@angular/common/http';
import { Game } from '../_models/game';
import { Observable } from 'rxjs';
import { IPlayer } from '../_models/IPlayer';
import { AuthService, User } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root',
})
export class GameHubService {
  private hubConnection?: HubConnection;
  private hubUrl = env.api.hubUrl;
  private apiUrl = env.api.apiUrl;

  constructor(
    private busyService: BusyService,
    private http: HttpClient,
    private auth: AuthService
  ) {}

  startConnection(user: User, gameName: string) {
    // TODO: FIX THE AUTHENTICATION
    /*     this.busyService.busy();
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}/gamehub?game=${gameName}`, {
        accessTokenFactory: () =>
          this.auth.getAccessTokenSilently().subscribe(),
        transport: HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(err => console.error(err))
      .finally(() => this.busyService.idle()); */
  }

  getHubConnection(): HubConnection {
    if (!this.hubConnection) {
      throw Error('Gamehub connection is not initialized');
    }
    return this.hubConnection;
  }

  getNotStartedGames(): Observable<Game[]> {
    return this.http.get<Game[]>(`${this.apiUrl}/game/not-started`);
  }

  getPlayersInGame(gameName: string): Observable<IPlayer[]> {
    return this.http.get<IPlayer[]>(`${this.apiUrl}/game/players/${gameName}`);
  }

  getGame(gameName: string): Observable<Game> {
    return this.http.get<Game>(`${this.apiUrl}/game/${gameName}`);
  }
}
