import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { HttpClientModule } from '@angular/common/http';
import { faInfoCircle, faCaretRight, faCaretLeft, faSun, faCog, faTimes, faTag, faAlignJustify, faArrowRight, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

// Pages
import { HomePage } from './pages/home-page/home.page';
import { TestTablePage } from './pages/test-table-page/test-table.page';
import { ExcelTestPage } from './pages/excel-test-page/excel-test.page';
import { UpdatePage } from './pages/update-page/update.page';

// Services
import { LoadingScreenService } from './services/loading-screen-service/loading-screen.service';
import { RouterService } from './services/router-service/router.service';
import { TestTableService } from './services/test-table-service/test-table.service';
import { BaseHttpService } from './services/base-http.service';
import { BankRowService } from './services/bank-row-service/bank-row.service';

// Components
import { AppComponent } from './app.component';
import { NavComponent } from './components/nav/nav.component';
import { LoadingScreenComponent } from './components/loading-screen/loading-screen.component';
import { WizardNavigationComponent } from './components/wizard-navigation/wizard-navigation.component';
import { WizardTestComponent } from './components/wizard-test/wizard-test.component';
import { ChooseFileComponent } from './components/update-wizard/step-1/choose-file/choose-file.component';
import { ReadFilesComponent } from './components/update-wizard/step-2/read-files/read-files.component';
import { EvalTransactionsComponent } from './components/update-wizard/step-3/eval-transactions/eval-transactions.component';
import { FooterComponent } from './components/footer/footer.component';

@NgModule({
  declarations: [
    // Pages
    HomePage,
    TestTablePage,
    ExcelTestPage,
    UpdatePage,
    // Components
    AppComponent,
    NavComponent,
    LoadingScreenComponent,
    WizardNavigationComponent,
    WizardTestComponent,
    ChooseFileComponent,
    FooterComponent,
    ReadFilesComponent,
    EvalTransactionsComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FontAwesomeModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomePage },
      { path: 'testtable', component: TestTablePage },
      { path: 'update', component: UpdatePage },
      { path: 'excel', component: ExcelTestPage },
    ], { useHash: true })
  ],
  providers: [
    LoadingScreenService,
    RouterService,
    BaseHttpService,
    TestTableService,
    BankRowService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor(library: FaIconLibrary) {
    // library.addIconPacks(fas);
    library.addIcons(faInfoCircle);
    library.addIcons(faCog);
    library.addIcons(faAlignJustify);
    library.addIcons(faTag);
    library.addIcons(faArrowLeft);
    library.addIcons(faArrowRight);
  }
}
