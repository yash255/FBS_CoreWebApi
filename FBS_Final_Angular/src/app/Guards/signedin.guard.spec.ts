import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { signedinGuard } from './signedin.guard';

describe('signedinGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => signedinGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
