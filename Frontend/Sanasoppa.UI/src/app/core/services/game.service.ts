import { BusyService } from '@app/core';
import { Injectable } from '@angular/core';
import { environment as env } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Game, Player } from '@app/core';
@Injectable({
  providedIn: 'root',
})
export class GameService {
  private apiUrl = env.api.apiUrl;

  constructor(private busyService: BusyService, private http: HttpClient) {}

  getNotStartedGames(): Observable<Game[]> {
    return this.http.get<Game[]>(`${this.apiUrl}/game/not-started`);
  }

  getPlayersInGame(gameName: string): Observable<Player[]> {
    return this.http.get<Player[]>(`${this.apiUrl}/game/players/${gameName}`);
  }

  getGame(gameName: string): Observable<Game> {
    return this.http.get<Game>(`${this.apiUrl}/game/${gameName}`);
  }
}
