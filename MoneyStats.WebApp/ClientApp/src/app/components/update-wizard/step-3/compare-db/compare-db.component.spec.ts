import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompareDbComponent } from './compare-db.component';

describe('CompareDbComponent', () => {
  let component: CompareDbComponent;
  let fixture: ComponentFixture<CompareDbComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompareDbComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompareDbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
