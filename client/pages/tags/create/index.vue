<template>
    <div class="container top-container">
      <div class="create-tag-container">
        <!-- Header with a back button and title -->
        <div
          class="create-tag-header d-flex align-items-center justify-content-start"
        >          
          <button @click="goBack" class="back-icon">
            <img src="/back-icon.png" alt="Back Icon" />
          </button>          
          <h2 class="text-center flex-grow-1">Create New Tag</h2>
        </div>
  
        <div class="card create-tag-card">
          <div class="card-body">
            <!-- Form to create a new tag -->
            <form @submit.prevent="handleSave">
              <div class="form-group">
                <label for="name" class="form-label">Tag Name</label>
                <input
                  v-model="tag.name"
                  type="text"
                  class="form-control"
                  id="name"
                  name="name"
                  placeholder="Enter tag name"
                  required
                />
              </div>
  
              <!-- Save button -->
              <button
                :disabled="isSaving"
                type="submit"
                class="btn btn-save-tag mt-4"
              >
                {{ isSaving ? "Saving..." : "Save Tag" }}
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import { getTags, createTag } from "~/services/tagService"; 
  import Multiselect from "vue-multiselect";
  import Swal from "sweetalert2";
  
  export default {
    components: { Multiselect }, 
    data() {
      return {
        tag: {
          name: "", // Tag name input field data
        },
        isSaving: false, // For handling the saving state of the form
      };
    },
  
    methods: {
      // Function to save the tag
      handleSave() {
        this.isSaving = true; // Set saving state to true
  
        // Call API to create the tag
        createTag(this.tag)
          .then(() => {
            Swal.fire({
              icon: "success",
              title: "Tag created successfully!",
              showConfirmButton: false,
              timer: 1500,
            });
  
            // Reset the form and redirect to the tags list
            this.tag = { name: "" };
            this.isSaving = false;
            this.$router.push("/tags");
          })
          .catch((error) => {
            console.error(error); 
            Swal.fire({
              icon: "error",
              title: "An Error Occurred!",
              showConfirmButton: false,
              timer: 1500,
            });
            this.isSaving = false; // Reset saving state on error
          });
      },
  
      // Function to navigate back
      goBack() {
        this.$router.go(-1);
      },
    },
  };
  </script>
  
  <style scoped>
  /* Styling for the top container */
  .top-container {
    margin-top: 5rem;
  }
  
  /* Header styling for the create tag page */
  .create-tag-header {
    display: flex;
    align-items: center;
    color: white;
    padding: 20px 40px;
    border-radius: 8px 8px 0 0;
    text-align: center;
    position: relative;
    margin-bottom: 25px;
  }
  
  /* Title styling */
  .create-tag-header h2 {
    margin: 0;
    font-size: 1.5rem;
    color: #052e82;
    flex-grow: 1;
    text-align: center;
  }
  
  /* Styling for the back button */
  .back-icon {
    margin-right: 10px;
    font-size: 24px;
    color: black;
    cursor: pointer;
    transition: background-color 0.3s, border 0.3s;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #052e82;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  }
  
  /* Hover effect for the back button */
  .back-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
  /* Icon inside back button */
  .back-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  
  /* Styling for the card container */
  .create-tag-card {
    margin-top: -20px;
    border-radius: 12px;
    border: none;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
    padding: 20px;
  }
  
  /* Input field styling */
  .form-control {
    border: none;
    border-radius: 8px;
    box-shadow: 0 12px 18px rgba(0, 0, 0, 0.05);
    padding: 12px 15px;
    margin-bottom: 20px;
  }
  
  /* Label styling */
  .form-label {
    margin: 5px;
  }
  
  /* Styling for save button */
  .btn-save-tag {
    background-color: #052e82;
    color: white;
    border: none;
    padding: 12px 20px;
    border-radius: 8px;
    cursor: pointer;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }
  
  /* Hover effect for save button */
  .btn-save-tag:hover {
    background-color: #3a3f6a;
  }
  
  </style>
  