export class Person {
    name: string;
    gender: string;
    age: number;
    pets: Pet[]

}


export class Pet {
    name: string;
    type: string;
}

export class PetDisplayDto {
    ownersGender: string;
    petNames: string[];
}