import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadFilesComponent } from './read-files.component';

describe('ReadFilesComponent', () => {
  let component: ReadFilesComponent;
  let fixture: ComponentFixture<ReadFilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadFilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
