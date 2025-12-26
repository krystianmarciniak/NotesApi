# 📝 Notes API – REST

Projekt przedstawia prostą aplikację typu REST API umożliwiającą
rejestrację użytkowników, logowanie z wykorzystaniem tokenów JWT
oraz zarządzanie prywatnymi notatkami użytkownika.

Aplikacja została przygotowana w celach dydaktycznych jako demonstracja:
- autoryzacji JWT,
- ochrony endpointów,
- powiązania danych z zalogowanym użytkownikiem,
- testowania API przy użyciu narzędzia Postman.

---

## ⚙️ Wykorzystane technologie

- **ASP.NET Core** – backend REST API  
- **JWT (JSON Web Token)** – mechanizm autoryzacji  
- **Entity Framework Core** – warstwa dostępu do danych  
- **Postman** – testowanie endpointów API  

---

## 🔐 Autoryzacja

API wykorzystuje mechanizm **JWT Bearer Token**.

1. Użytkownik loguje się za pomocą endpointu `/login`
2. Serwer zwraca token JWT
3. Token jest przekazywany w nagłówku:

Authorization: Bearer <token>
4. Dostęp do endpointów chronionych jest możliwy tylko po poprawnej autoryzacji

## 🧪 Scenariusz testowy API (Postman)

Poniżej przedstawiono kompletny scenariusz testowy REST API obejmujący:
rejestrację użytkownika, logowanie, autoryzację JWT oraz operacje na notatkach.

### 1 Rejestracja użytkownika  

**Plik:**`(./screenshots/01_register_userA_200.png)`

Rejestracja nowego użytkownika w systemie przy użyciu endpointu `/register`. Operacja zakończona kodem odpowiedzi 200 OK.

### 2 Logowanie użytkownika – uzyskanie tokena JWT   

**Plik:**`(./screenshots/02_login_userA_200_jwt.png)`

Logowanie użytkownika przy użyciu endpointu `/login`.
W odpowiedzi serwer zwraca poprawny token JWT.

### 3  Użycie tokena JWT w nagłówku Authorization.

**Plik:**`(./screenshots/02_login_userA_Authorization_Bearer_Token_200_jwt.png)`

Przekazanie otrzymanego tokena JWT w nagłówku
`Authorization: Bearer <token>` w narzędziu Postman.

### 4  Pobranie listy notatek – stan początkowy  

**Plik:**`(./screenshots/03_get_notes_userA_empty_200_b.png)`

Pobranie listy notatek zalogowanego użytkownika przy użyciu endpointu `/notes`.
Lista jest pusta, co potwierdza brak danych początkowych.

### 5 Dodanie nowej notatki  

**Plik:**`(./screenshots/04_post_notes_userA_200.png)`

Dodanie nowej notatki przez zalogowanego użytkownika przy użyciu endpointu `/notes`.
Operacja zakończona sukcesem (**200 OK**).

### 6 Pobranie listy notatek po dodaniu elementu  

**Plik:**`(./screenshots/05_get_notes_userA_1item_200.png)`

Ponowne pobranie listy notatek.
Odpowiedź zawiera wcześniej dodaną notatkę, co potwierdza poprawne
powiązanie danych z użytkownikiem.

## Uwagi techniczne

- Endpointy chronione wymagają poprawnego tokena JWT
- Dane notatek są widoczne wyłącznie dla użytkownika, który je utworzył
- Żądania typu GET nie wymagają przesyłania danych w Body
- Testy wykonano lokalnie przy użyciu Postman

---

## Podsumowanie

Projekt demonstruje poprawne działanie REST API z autoryzacją JWT
oraz pełny przepływ użytkownika:
rejestracja → logowanie → autoryzacja → operacje CRUD na danych prywatnych.

Aplikacja spełnia założenia projektu dydaktycznego i została
przetestowana przy użyciu rzeczywistych żądań HTTP.