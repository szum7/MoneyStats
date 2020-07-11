import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvalTransactionsComponent } from './eval-transactions.component';

describe('EvalTransactionsComponent', () => {
  let component: EvalTransactionsComponent;
  let fixture: ComponentFixture<EvalTransactionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvalTransactionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvalTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
