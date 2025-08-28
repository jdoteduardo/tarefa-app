import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormTarefasComponent } from './form-tarefas.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TarefaService } from '../../../services/tarefa.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';

describe('FormTarefasComponent', () => {
  let component: FormTarefasComponent;
  let fixture: ComponentFixture<FormTarefasComponent>;
  let tarefaServiceSpy: jasmine.SpyObj<TarefaService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let activatedrouterSpy: jasmine.SpyObj<ActivatedRoute>;

  beforeEach(async () => {
    const id = crypto.randomUUID();
    tarefaServiceSpy = jasmine.createSpyObj('TarefaService', ['salvarTarefa', 'editarTarefa', 'buscarTarefaPorId']);
    tarefaServiceSpy.salvarTarefa.and.returnValue(of({
      id: crypto.randomUUID(),
      titulo: 'Nova Tarefa',
      descricao: 'Descrição da tarefa',
      dataLimite: new Date(),
      concluida: false
    }));
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    activatedrouterSpy = {
    snapshot: {
        paramMap: {
            get: (key: string) => null
        }
      }
    } as any;

    await TestBed.configureTestingModule({
      imports: [
        FormTarefasComponent,
        ReactiveFormsModule,
        HttpClientTestingModule
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: activatedrouterSpy
        },
        {
          provide: TarefaService,
          useValue: tarefaServiceSpy
        },
        { 
            provide: Router, 
            useValue: routerSpy 
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(FormTarefasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should return tarefa por id on ngOnInit if id is present', () => {
    const id = crypto.randomUUID();
    activatedrouterSpy.snapshot = {
      paramMap: {
        get: (key: string) => key === 'id' ? id : null
      }
    } as any;
    const mockTarefa = { id, titulo: 'Test', descricao: 'Test Desc', dataLimite: new Date(), concluida: false };
    tarefaServiceSpy.buscarTarefaPorId.and.returnValue(of(mockTarefa));
    component.ngOnInit();
    expect(component.acao).toBe('Editar');
    expect(component.tarefa).toEqual(mockTarefa);
    expect(tarefaServiceSpy.buscarTarefaPorId).toHaveBeenCalledWith(id);
  });

  it('should set acao to Criar on ngOnInit if no id is present', () => {
    activatedrouterSpy.snapshot = {
      paramMap: {
        get: (key: string) => null
      }
    } as any;
    component.ngOnInit();
    expect(component.acao).toBe('Criar');
    expect(component.tarefa).toBeNull();
  });

  it('should call cancelar.emit on onCancelar', () => {
    spyOn(component.cancelar, 'emit');
    component.onCancelar();
    expect(component.cancelar.emit).toHaveBeenCalled();
  });

  it('should call salvarOuEditarTarefa.emit on onSalvarOuEditarTarefa if form is valid and acao is Criar', () => {
    spyOn(component.salvarOuEditarTarefa, 'emit');
    component.acao = 'Criar';

    tarefaServiceSpy.salvarTarefa.and.returnValue(of({
      id: crypto.randomUUID(),
      titulo: 'Nova Tarefa',
      descricao: 'Descrição da tarefa',
      dataLimite: new Date(),
      concluida: false
    }));

    component.tarefaForm.setValue({
      TITULO: 'Nova Tarefa',
      DESCRICAO: 'Descrição da tarefa',
      DATA_LIMITE: new Date(),
      CONCLUIDA: false
    });

    component.onSubmit();

    expect(component.salvarOuEditarTarefa.emit).toHaveBeenCalled();
    expect(tarefaServiceSpy.salvarTarefa).toHaveBeenCalled();
  });

  it('should call salvarOuEditarTarefa.emit on onSalvarOuEditarTarefa if form is valid and acao is Editar', () => {
    spyOn(component.salvarOuEditarTarefa, 'emit');
    component.acao = 'Editar';
    const id = crypto.randomUUID();
    component.tarefa = {
      id,
      titulo: 'Tarefa Existente',
      descricao: 'Descrição existente',
      dataLimite: new Date(),
      concluida: false
    };

    tarefaServiceSpy.editarTarefa.and.returnValue(of({
      id,
      titulo: 'Tarefa Atualizada',
      descricao: 'Descrição atualizada',
      dataLimite: new Date(),
      concluida: true
    }));

    component.tarefaForm.setValue({
      TITULO: 'Tarefa Atualizada',
      DESCRICAO: 'Descrição atualizada',
      DATA_LIMITE: new Date(),
      CONCLUIDA: true
    });

    component.onSubmit();

    expect(component.salvarOuEditarTarefa.emit).toHaveBeenCalled();
    expect(tarefaServiceSpy.editarTarefa).toHaveBeenCalledWith(jasmine.any(Object));
  });
});