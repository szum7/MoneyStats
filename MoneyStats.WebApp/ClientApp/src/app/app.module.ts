import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { HttpClientModule } from '@angular/common/http';
import { faChevronDown, faChevronUp, faMinus, faPlus, faWrench, faCheck, faTimes, faInfoCircle, faEye, faEyeSlash, faCog, faTag, faAlignJustify, faArrowRight, faArrowLeft, faChartBar, faChartPie, IconDefinition } from '@fortawesome/free-solid-svg-icons';
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
import { RuleService } from './services/rule-service/rule.service';
import { GeneratedTransactionService } from './services/generated-transaction-service/generated-transaction.service';
import { TagService } from './services/tag-service/tag.service';

// Components
import { AppComponent } from './app.component';
import { NavComponent } from './components/nav/nav.component';
import { LoadingScreenComponent } from './components/loading-screen/loading-screen.component';
import { WizardNavigationComponent } from './components/wizard-navigation/wizard-navigation.component';
import { ChooseFileComponent } from './components/update-wizard/step-1/choose-file/choose-file.component';
import { ReadFilesComponent } from './components/update-wizard/step-2/read-files/read-files.component';
import { EvalTransactionsComponent } from './components/update-wizard/step-4/eval-transactions/eval-transactions.component';
import { FooterComponent } from './components/footer/footer.component';
import { WizardNavComponent } from './components/wizard-nav/wizard-nav.component';
import { CompareDbComponent } from './components/update-wizard/step-3/compare-db/compare-db.component';

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
    ChooseFileComponent,
    FooterComponent,
    ReadFilesComponent,
    EvalTransactionsComponent,
    WizardNavComponent,
    CompareDbComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FontAwesomeModule,
    NgbModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomePage },
      { path: 'test', component: TestTablePage },
      { path: 'update', component: UpdatePage },
      { path: 'excel', component: ExcelTestPage },
    ], { useHash: true })
  ],
  providers: [
    LoadingScreenService,
    RouterService,
    BaseHttpService,
    TestTableService,
    BankRowService,
    RuleService,
    GeneratedTransactionService,
    TagService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(library: FaIconLibrary) {

    let icons: IconDefinition[] = [
      faInfoCircle, faCog, faAlignJustify, faTag,
      faArrowLeft, faArrowRight,
      faChartPie, faChartBar,
      faEye, faEyeSlash, faCheck, faTimes, faWrench, faPlus, faMinus, faChevronDown, faChevronUp
    ];

    this.initIcons(library, icons);
  }

  private initIcons(library: FaIconLibrary, icons: IconDefinition[]): void {    
    for (let i = 0; i < icons.length; i++) {
      library.addIcons(icons[i]);
    }
    //library.addIconPacks(fas);
  }

}
