
package cz.fi.muni.pb138.familytree;

import java.time.LocalDate;
import java.util.Objects;

/**
 *
 * @author Tran Manh Hung 433556
 */
public class Person {

	private Long id;
	private String name;
	private String surname;
	private LocalDate birthDate;
	private Person father;
	private Person mother;
	private LocalDate deathDate;

	public Person getMother() {
		return this.mother;
	}

	public void setMother(Person mother) {
		this.mother = mother;
	}

	public Person getFather() {
		return this.father;
	}

	public void setFather(Person father) {
		this.father = father;
	}

	public LocalDate getBirthDate() {
		return this.birthDate;
	}

	public void setBirthDate(LocalDate birthDate) {
		this.birthDate = birthDate;
	}

	public String getSurname() {
		return this.surname;
	}

	public void setSurname(String surname) {
		this.surname = surname;
	}

	public String getName() {
		return this.name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Long getId() {
		return this.id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public LocalDate getDeathDate() {
		return this.deathDate;
	}

	public void setDeathDate(LocalDate deathDate) {
		this.deathDate = deathDate;
	}

    @Override
    public int hashCode() {
        int hash = 7;
        hash = 67 * hash + Objects.hashCode(this.id);
        return hash;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj == null) {
            return false;
        }
        if (getClass() != obj.getClass()) {
            return false;
        }
        final Person other = (Person) obj;
        return Objects.equals(this.id, other.id);
    }

    @Override
    public String toString() {
        return "Person{" + "id=" + id + ", name=" + name + ", surname=" + surname + ", birthDate=" + birthDate + ", father=" + father + ", mother=" + mother + ", deathDate=" + deathDate + '}';
    }
        
}
