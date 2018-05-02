import { Pipe, PipeTransform } from '@angular/core';
import { Person, Pet, PetDisplayDto } from '../Models/models';

@Pipe({ name: 'groupBy' })
export class GroupByPipe implements PipeTransform {

    transform(input: any, prop: string): Array<any> {

        if (!Array.isArray(input)) {
            return input;
        }

        const arr: { [key: string]: Array<any> } = {};

        for (const value of input) {
            const field: any = getProperty(value, prop);

            if (isUndefined(arr[field])) {
                arr[field] = [];
            }

            arr[field].push(value);
        }

        var result = Object.keys(arr).map(key => ({ key, 'value': arr[key] }));

        
        //var sortedResult = new Array<PetDisplayDto>();
        //for (let group of result) {
        //    var personData = new PetDisplayDto();
        //    personData.ownersGender = group.key;
        //    var pets = new Array<string>();

        //    for (let person of group.value) {
        //        for (let pet of person.pets) {

        //            pets.push(pet.name);
        //        }
        //    }
        //    pets.sort();
        //    personData.petNames = pets;
        //    sortedResult.push(personData);
        //}
        //return sortedResult;
        return result;
    }
}

export function isUndefined(value: any): value is undefined {

    return typeof value === 'undefined';
}

export function getProperty(value: { [key: string]: any }, key: string): any {

    if (isNil(value) || !isObject(value)) {
        return undefined;
    }

    const keys: string[] = key.split('.');
    let result: any = value[keys.shift()!];

    for (const key of keys) {
        if (isNil(result) || !isObject(result)) {
            return undefined;
        }

        result = result[key];
    }

    return result;
}

export function isNil(value: any): value is (null | undefined) {
    return value === null || typeof (value) === 'undefined';
}
export function isObject(value: any): boolean {

    return typeof value === 'object';
}