import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ListTarefasComponent } from './list-tarefas.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TarefaService } from '../../../services/tarefa.service';
import { of } from 'rxjs';
import { ITarefa } from '../../../interfaces/ITarefa';
import { Guid } from 'devextreme/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Router } from '@angular/router';

describe('ListTarefasComponent', () => {
  let component: ListTarefasComponent;
  let fixture: ComponentFixture<ListTarefasComponent>;
  let tarefaServiceSpy: jasmine.SpyObj<TarefaService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    tarefaServiceSpy = jasmine.createSpyObj('TarefaService', ['listarTarefas', 'deletarTarefa']);
    tarefaServiceSpy.listarTarefas.and.returnValue(of([]));
    tarefaServiceSpy.deletarTarefa.and.returnValue(of({
        id: crypto.randomUUID(),
        titulo: '',
        descricao: '',
        dataLimite: new Date(),
        concluida: false
    }));
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [
        ListTarefasComponent, 
        ReactiveFormsModule,
        BrowserAnimationsModule
      ],
      providers: [
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

    fixture = TestBed.createComponent(ListTarefasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should return tarefas on listarTarefas', () => {
    const mockTarefas: ITarefa[] = [
      { id: crypto.randomUUID(), titulo: 'Tarefa 1', descricao: 'Desc 1', dataLimite: new Date(), concluida: false },
      { id: crypto.randomUUID(), titulo: 'Tarefa 2', descricao: 'Desc 2', dataLimite: new Date(), concluida: true }
    ];
    tarefaServiceSpy.listarTarefas.and.returnValue(of(mockTarefas));

    component.listarTarefas();

    expect(component.tarefas.length).toBe(2);
    expect(component.dataSource.data).toEqual(mockTarefas);
  });

  it('should navigate to tarefas/nova on adicionarTarefa', () => {
    component.adicionarTarefa();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/tarefas/nova']);
  });

  it('should navigate to tarefas/id on editarTarefa', () => {
    const tarefa: ITarefa = { id: crypto.randomUUID(), titulo: 'Tarefa 1', descricao: 'Desc 1', dataLimite: new Date(), concluida: false };
    component.editarTarefa(tarefa);
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/tarefas', tarefa.id]);
  });

  it('should delete tarefa on confirmarDelete and called listarTarefas', () => {
    const tarefa: ITarefa = { id: crypto.randomUUID(), titulo: 'Tarefa 1', descricao: 'Desc 1', dataLimite: new Date(), concluida: false };
    component.deletarTarefa(tarefa);
    component.confirmarDelete();
    expect(tarefaServiceSpy.deletarTarefa).toHaveBeenCalledWith(tarefa.id!);
    expect(tarefaServiceSpy.listarTarefas).toHaveBeenCalled();
  });
});