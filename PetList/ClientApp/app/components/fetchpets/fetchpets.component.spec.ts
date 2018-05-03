/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { assert } from 'chai';
import { FetchPetsComponent } from './fetchpets.component';
import { TestBed, async, ComponentFixture, inject, tick, fakeAsync} from '@angular/core/testing';
import { MockBackend } from '@angular/http/testing';
import { BaseRequestOptions, Http, XHRBackend, HttpModule, Response, ResponseOptions } from '@angular/http';
import { Person, Pet, PetDisplayDto } from '../Models/models';

//Setup
let fixture: ComponentFixture<FetchPetsComponent>;
describe('FetchPetsComponent component', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                MockBackend,
                BaseRequestOptions,
                {
                    provide: Http,
                    useFactory: (backend: any, options: any) => new Http(backend, options),
                    deps: [MockBackend, BaseRequestOptions]
                },
                { provide: 'BASE_URL', useValue: 'http://test.com.au/' }
            ],
            imports: [
                HttpModule
            ],
            declarations: [FetchPetsComponent]
        });
        fixture = TestBed.createComponent(FetchPetsComponent);
        fixture.detectChanges();
    });

    //initial test to verify the HTML rendering 
    it('should display a title', async(() => {
            const titleText = fixture.nativeElement.querySelector('h1').textContent;
        expect(titleText).toEqual('People with Cats');

    }));
    //Test to verify the grouping by owners gender
    it('should fetch data from server and render correctly', fakeAsync(inject(

        [MockBackend], (mockBackend: any) => {
            let mockResponse =
            [
                { name: "Anna", gender: "Female", pets: [] },
                { name: "Peter", gender: "Male", pets: []},
                { name: 'Alex', gender: "Male", pets: []},
                { name: 'Lynn', gender: "Female", pets: [] }
            ];
            
            mockBackend.connections.subscribe((connection: any) => {
                connection.mockRespond(new Response(new ResponseOptions({ 
                    body: JSON.stringify(mockResponse)
                })));
            });
            fixture.debugElement.componentInstance.fetchData();
            tick();
            expect(fixture.debugElement.componentInstance.persons.length).toBe(4);
            expect(fixture.debugElement.componentInstance.petDisplayResults.length).toBe(2);
            fixture.detectChanges();
            const groupHeader = fixture.nativeElement.querySelectorAll('h2');
            expect(groupHeader.length).toEqual(2);
        })));

    //Test to verify the sorting of the pet names

    it('should fetch data from server and sort the pet based on its name correctly', fakeAsync(inject(

        [MockBackend], (mockBackend: any) => {
            let mockResponse =
                [
                    { name: "Anna", gender: "Female", pets: [{ name: "b", type: "cat" }, { name: "f", type: "cat" }] },
                    { name: "Anna", gender: "Female", pets: [{ name: "c", type: "cat" }, { name: "a", type: "cat" }] },

                ];

            mockBackend.connections.subscribe((connection: any) => {
                connection.mockRespond(new Response(new ResponseOptions({
                    body: JSON.stringify(mockResponse)
                })));
            });
            fixture.debugElement.componentInstance.fetchData();
            tick();
            //expect(fixture.debugElement.componentInstance.persons.length).toBe(4);
            expect(fixture.debugElement.componentInstance.petDisplayResults.length).toBe(1);
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].ownersGender).toBe("Female");
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames.length).toBe(4);
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[0]).toBe("a");
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[1]).toBe("b");
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[2]).toBe("c");
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[3]).toBe("f");
            fixture.detectChanges();

        })));

    //Sort with multiple owner type
    it('should fetch data from server and sort the pet based on its name correctly [ case insensitive ( covertint ot lower case and sorting the data)', fakeAsync(inject(

        [MockBackend], (mockBackend: any) => {
            let mockResponse =
                [
                    { name: "Anna", gender: "Male", pets: [{ name: "z", type: "cat" }, { name: "f", type: "cat" }] },

                    { name: "Anna", gender: "Female", pets: [{ name: "b", type: "cat" }, { name: "f", type: "cat" }] },
                    { name: "Anna", gender: "Female", pets: [{ name: "c", type: "cat" }, { name: "a", type: "cat" }] },

                ];

            mockBackend.connections.subscribe((connection: any) => {
                connection.mockRespond(new Response(new ResponseOptions({
                    body: JSON.stringify(mockResponse)
                })));
            });
            fixture.debugElement.componentInstance.fetchData();
            tick();
            //expect(fixture.debugElement.componentInstance.persons.length).toBe(4);
            expect(fixture.debugElement.componentInstance.petDisplayResults.length).toBe(2);
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].ownersGender).toBe("Male");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].ownersGender).toBe("Female");

            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames.length).toBe(2);
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[0]).toBe("f");
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[1]).toBe("z");

            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames.length).toBe(4);
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[0]).toBe("a");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[1]).toBe("b");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[2]).toBe("c");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[3]).toBe("f");
            fixture.detectChanges();

        })));

    //Sort with multiple owner type. case sensitive data
    it('should fetch data from server and sort the pet based on its name correctly [ case sensitive ( covertint ot lower case and sorting the data)', fakeAsync(inject(

        [MockBackend], (mockBackend: any) => {
            let mockResponse =
                [
                    { name: "Anna", gender: "Male", pets: [{ name: "Z", type: "cat" }, { name: "f", type: "cat" }] },

                    { name: "Anna", gender: "Female", pets: [{ name: "b", type: "cat" }, { name: "f", type: "cat" }] },
                    { name: "Anna", gender: "Female", pets: [{ name: "c", type: "cat" }, { name: "a", type: "cat" }] },

                ];

            mockBackend.connections.subscribe((connection: any) => {
                connection.mockRespond(new Response(new ResponseOptions({
                    body: JSON.stringify(mockResponse)
                })));
            });
            fixture.debugElement.componentInstance.fetchData();
            tick();
            //expect(fixture.debugElement.componentInstance.persons.length).toBe(4);
            expect(fixture.debugElement.componentInstance.petDisplayResults.length).toBe(2);
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].ownersGender).toBe("Male");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].ownersGender).toBe("Female");

            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames.length).toBe(2);
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[0]).toBe("Z"); // when pusing to the array if we update the data to lower case then this can be solved. or we need to introduce custom sort intsted of the arryay. sort.
                                                                                                       // Need to confirm from business for the actual requirement
            expect(fixture.debugElement.componentInstance.petDisplayResults[0].petNames[1]).toBe("f");

            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames.length).toBe(4);
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[0]).toBe("a");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[1]).toBe("b");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[2]).toBe("c");
            expect(fixture.debugElement.componentInstance.petDisplayResults[1].petNames[3]).toBe("f");
            fixture.detectChanges();

        })));
});
