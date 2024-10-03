<template>
    <div class="container top-container">
      
        <!-- Container for editing an existing tag -->
      <div class="edit-tag-container">
        <div class="card edit-tag-card">
          <div class="edit-tag-header">
            <button @click="goBack" class="back-icon">
              <img src="/back-icon.png" alt="Back Icon" />
            </button>
            <h2 class="text-center">Update Tag</h2>
          </div>

          <!-- Form to update the tag -->
          <div class="card-body">
            <form @submit.prevent="handleSave">
              <div class="form-group">      
                <label for="name" class="form-label">Name</label>
                <input
                  v-model="tag.name"
                  type="text"
                  class="form-control"
                  id="name"
                  name="name"
                  required
                />
              </div>
  
              <!-- Button to save the updated tag -->
              <button
                @click="handleSave()"
                :disabled="isSaving"
                type="submit"
                class="btn btn-save-tag mt-4"
              >
                {{ isSaving ? "Updating..." : "Update Tag" }}
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import Swal from "sweetalert2"; 
  import { getTag, updateTag } from "~/services/tagService"; 
  
  export default {
    data() {
      return {
        tag: {
          name: "", 
        },
        isSaving: false, 
      };
    },
    created() {
      // Get the tag ID from route params and fetch tag details
      const id = this.$route.params.id;
      getTag(id)
        .then((response) => {
          const tagInfo = response.data;
          this.tag.name = tagInfo.name; 
        })
        .catch((error) => {
          Swal.fire({
            icon: "error",
            title: "An Error Occurred!",
            showConfirmButton: false,
            timer: 1500,
          });
        });
    },
    methods: {
      // Function to save the updated tag
      handleSave() {
        this.isSaving = true; 
        const id = this.$route.params.id; 
  
        // Call the API to update the tag
        updateTag(id, this.tag)
          .then((response) => {
            Swal.fire({
              icon: "success",
              title: "Tag updated successfully!",
              showConfirmButton: false,
              timer: 1500,
            });
            this.isSaving = false;
            this.tag.name = ""; 
            this.$router.go(-1); 
          })
          .catch((error) => {
            this.isSaving = false; 
            Swal.fire({
              icon: "error",
              title: "An Error Occurred!",
              showConfirmButton: false,
              timer: 1500,
            });
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
  /* Top margin for the container */
  .top-container {
    margin-top: 5rem;
  }
  
  /* Styling for the edit tag header */
  .edit-tag-header {
    background: linear-gradient(135deg, #3a3f6a, #052e82);
    color: white;
    padding: 40px;
    border-radius: 8px 8px 0 0;
    text-align: center;
    position: relative;
  }
  
  /* Styling for the back button */
  .back-icon {
    position: absolute;
    left: 20px;
    top: 50%;
    transform: translateY(-50%);
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
    border: 2px solid white;
  }
  
  /* Hover effect for the back button */
  .back-icon:hover {
    background-color: #052e82;
    color: white;
    border-color: white;
  }
  
  /* Styling for the back icon image */
  .back-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  
  /* Styling for the card header */
  .card-header {
    background: linear-gradient(135deg, #3a3f6a, #052e82);
    color: white;
    padding: 20px;
    border-radius: 8px 8px 0 0;
  }
  
  /* Styling for form labels */
  .form-label {
    font-weight: bold;
    color: #052e82;
  }
  
  /* Styling for the save button */
  .btn-save-tag {
    background-color: #052e82;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 4px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  /* Hover effect for the save button */
  .btn-save-tag:hover {
    background-color: #3a3f6a;
  }
  </style>
  