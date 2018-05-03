import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { GroupByPipe, getProperty, isUndefined} from '../Utils/utils';
import { Person, Pet, PetDisplayDto } from '../Models/models';


@Component({
    selector: 'fetchpets',
    templateUrl: './fetchpets.component.html'
})

export class FetchPetsComponent {
    public persons: Person[];
    public petDisplayResults: PetDisplayDto[];
    private http: Http;
    private baseUrl: string;
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.baseUrl = baseUrl;
        this.http = http;
        this.fetchData();
    }

    fetchData() {
        this.http.get(this.baseUrl + 'api/Person/GetPersons?petType=cat').subscribe(result => {

            this.persons = result.json() as Person[];

            const arr: { [key: string]: Array<any> } = {};

            for (const value of this.persons) {
                const field: any = getProperty(value, "gender");

                if (isUndefined(arr[field])) {
                    arr[field] = [];
                }

                arr[field].push(value);
            }

            var groupedResult = Object.keys(arr).map(key => ({ key, 'value': arr[key] }));


            var sortedResult = new Array<PetDisplayDto>();
            for (let group of groupedResult) {
                var personData = new PetDisplayDto();
                personData.ownersGender = group.key;
                var pets = new Array<string>();

                for (let person of group.value) {
                    for (let pet of person.pets) {

                        pets.push(pet.name);
                    }
                }
                pets.sort();
                personData.petNames = pets;
                sortedResult.push(personData);
            }
            this.petDisplayResults = sortedResult;

        }, error => console.error(error));

    }
}


