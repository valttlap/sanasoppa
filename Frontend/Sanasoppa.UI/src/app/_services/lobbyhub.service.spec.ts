import { TestBed } from '@angular/core/testing';

import { LobbyHubService } from './lobbyhub.service';

describe('LobbyhubService', () => {
  let service: LobbyHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LobbyHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
