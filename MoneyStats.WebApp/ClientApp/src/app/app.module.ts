import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { HttpClientModule } from '@angular/common/http';
import { 
  faTrashAlt, faChevronDown, faChevronUp, faMinus, 
  faPlus, faWrench, faCheck, faTimes, faInfoCircle, 
  faEye, faEyeSlash, faCog, faTag, faAlignJustify, 
  faArrowRight, faArrowLeft, faChartBar, faChartPie, 
  IconDefinition, faBan, faFeather, faUniversity, 
  faAngleDoubleRight, faClipboardList, faClipboardCheck } from '@fortawesome/free-solid-svg-icons';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

// Pages
import { HomePage } from './pages/home-page/home.page';
import { TestTablePage } from './pages/test-table-page/test-table.page';
import { UpdatePage } from './pages/update-page/update.page';
import { ManageRulesPage } from './pages/manage-rules/manage-rules.page';
import { StatisticsPage } from './pages/statistics/statistics.page';

// Services
import { LoadingScreenService } from './services/loading-screen-service/loading-screen.service';
import { RouterService } from './services/router-service/router.service';
import { TestTableService } from './services/test-table.service';
import { BaseHttpService } from './services/base-http.service';
import { BankRowService } from './services/bank-row.service';
import { RuleService } from './services/rule.service';
import { GeneratedTransactionService } from './services/generated-transaction.service';
import { TagService } from './services/tag.service';
import { TransactionService } from './services/transaction.service';
import { StatisticsService } from './services/statistics.service';

// Components
import { AppComponent } from './app.component';
import { NavComponent } from './components/nav/nav.component';
import { LoadingScreenComponent } from './components/loading-screen/loading-screen.component';
import { WizardNavigationComponent } from './components/wizard-navigation/wizard-navigation.component';
import { ChooseFileComponent } from './components/wizards/input-bankrows-wizard/choose-file/choose-file.component';
import { ReadFilesComponent } from './components/wizards/input-bankrows-wizard/read-files/read-files.component';
import { EvalTransactionsComponent } from './components/wizards/create-transactions-wizard/eval-transactions/eval-transactions.component';
import { FooterComponent } from './components/footer/footer.component';
import { CompareDbComponent } from './components/wizards/input-bankrows-wizard/compare-db/compare-db.component';
import { EditRulesComponent } from './components/edit-rules/edit-rules.component';
import { SelectComponent } from './components/select/select.component';
import { DropdownComponent } from './components/dropdown/dropdown.component';
import { BasicMonthlyBarchartComponent } from './components/statistics/basic-monthly-barchart/basic-monthly-barchart.component';
import { BasicMonthlyBarchartV2Component } from './components/statistics/basic-monthly-barchart-v2/basic-monthly-barchart-v2.component';
import { ReversePipe } from './pipes/reverse.pipe';
import { WizardNavigatorComponent } from './components/wizards/wizard-navigator/wizard-navigator.component';

@NgModule({
  declarations: [
    // Pages
    HomePage,
    TestTablePage,
    UpdatePage,
    ManageRulesPage,
    StatisticsPage,
    // Components
    AppComponent,
    NavComponent,
    LoadingScreenComponent,
    WizardNavigationComponent,
    ChooseFileComponent,
    FooterComponent,
    ReadFilesComponent,
    EvalTransactionsComponent,
    CompareDbComponent,
    EditRulesComponent,
    SelectComponent,
    DropdownComponent,
    BasicMonthlyBarchartComponent,
    BasicMonthlyBarchartV2Component,
    // Pipes
    ReversePipe,
    WizardNavigatorComponent,
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
      { path: 'input-bank-rows', component: UpdatePage },
      { path: 'rules', component: ManageRulesPage },
      { path: 'statistics', component: StatisticsPage },
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
    TagService,
    TransactionService,
    StatisticsService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(library: FaIconLibrary) {

    let icons: IconDefinition[] = [
      faInfoCircle, faCog, faAlignJustify, faTag,
      faArrowLeft, faArrowRight, faChartPie, faChartBar,
      faEye, faEyeSlash, faCheck, faTimes, 
      faWrench, faPlus, faMinus, faChevronDown, 
      faChevronUp, faTrashAlt, faBan, faFeather,
      faUniversity, faAngleDoubleRight, faClipboardList,
      faClipboardCheck
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
