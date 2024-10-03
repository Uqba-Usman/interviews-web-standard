## Setup Instructions

### 1. Setting Up the API (ASP.NET Core)

1. **Navigate to the API folder**:

   ```
   cd api
   ```

2. **Restore Dependencies**:

   ```
   dotnet restore
   ```

3. **Apply Migrations and Update the Database**:
   Ensure that the SQLite database is up to date with the schema by running migrations.

   ```
   dotnet ef database update
   ```

4. **Run the API**:
   Once the database is set up, start the API server.
   ```bash
   dotnet run
   ```
   The API should now be running on `http://localhost:5000` by default.

### 2. Setting Up the Client (Nuxt 3)

1. **Navigate to the client folder**:

   ```bash
   cd client
   ```

2. **Install Dependencies**:
   Install the necessary Nuxt and Vue.js packages.

   ```bash
   npm install
   ```

3. **Configure API Base URL**:
   In the `client/nuxt.config.js` file, set the API base URL to match the one used by your ASP.NET Core API (e.g., `http://localhost:5000`).

4. **Run the Client**:
   Start the development server for Nuxt.

   ```bash
   npm run dev
   ```

5. **Access the Client**:
   The Nuxt application will be available at `http://localhost:3000`.
