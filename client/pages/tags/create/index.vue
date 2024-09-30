<template>
    <div class="container top-container">
      <div class="create-tag-container">
        <div class="create-tag-header d-flex align-items-center justify-content-start">
          <button @click="goBack" class="back-icon">
            <img src="/back-icon.png" alt="Back Icon" />
          </button>
          <h2 class="text-center flex-grow-1">Create New Tag</h2>
        </div>
  
        <div class="card create-tag-card">
          <div class="card-body">
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
  import {  getTags, createTag } from '~/services/tagService'; // Adjust imports as necessary
  import Multiselect from 'vue-multiselect';
  import Swal from 'sweetalert2';
  
  export default {
    components: { Multiselect },
    data() {
      return {
        tag: {
          name: '',
          
        },
        loading: true,
        isSaving: false,
      };
    },
    
    methods: {
   
      handleSave() {
        this.isSaving = true;
        
        createTag(this.tag)
          .then(() => {
            Swal.fire({
              icon: 'success',
              title: 'Tag created successfully!',
              showConfirmButton: false,
              timer: 1500,
            });
            // Reset form
            this.tag = { name: ''};            
            this.isSaving = false;
            this.$router.push("/tags")
            // Redirect or perform other actions
          })
          .catch((error) => {
            console.error(error);
            Swal.fire({
              icon: 'error',
              title: 'An Error Occurred!',
              showConfirmButton: false,
              timer: 1500,
            });
            this.isSaving = false;
          });
      },
      goBack() {
        this.$router.go(-1); // Navigate back to the previous page
      },
     
    },
  };
  </script>
  
  <style scoped>
  /* Layout adjustments to remove borders and add shadows */
  /* Main container adjustments */
  .top-container {
    margin-top: 5rem;
  }
  
  /* Header adjustment for flex alignment */
  .create-tag-header {
    display: flex;
    align-items: center;
    /* background: linear-gradient(135deg, #3a3f6a, #052e82); */
    color: white;
    padding: 20px 40px; /* Added padding for better spacing */
    border-radius: 8px 8px 0 0;
    text-align: center;
    position: relative;
    margin-bottom: 25px;
  }
  
  /* Flexbox adjustments for positioning */
  .create-tag-header h2 {
    margin: 0;
    font-size: 1.5rem;
    color: #052e82;
    flex-grow: 1; /* Ensures the text fills available space */
    text-align: center; /* Ensures the text stays centered */
  }
  
  .back-icon {
    margin-right: 10px; /* Adds a little space between the button and the text */
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
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1); /* Shadow for the floating effect */
  }
  
  .back-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
  .back-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  /* Card with shadow and without border */
  .create-tag-card {
    margin-top: -20px;
    border-radius: 12px;
    border: none; /* No border */
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1); /* Light shadow */
    padding: 20px;
  }
  
  /* Form inputs with light shadows and no borders */
  .form-control {
    border: none; /* No border */
    border-radius: 8px; /* Slight rounding */
    box-shadow: 0 12px 18px rgba(0, 0, 0, 0.05); /* Light shadow */
    padding: 12px 15px; /* Ensure inputs are comfortable to use */
    margin-bottom: 20px;
  }
  
  .form-label {
    margin:5px;
  }
  
  /* Save button with shadow */
  .btn-save-tag {
    background-color: #052e82;
    color: white;
    border: none;
    padding: 12px 20px;
    border-radius: 8px;
    cursor: pointer;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15); /* Stronger shadow for buttons */
  }
  
  .btn-save-tag:hover {
    background-color: #3a3f6a;
  }
  
  /* Tag chips */
  .tag-chips {
    display: flex;
    flex-wrap: wrap;
  }
  
  .tag-chip {
    background-color: #305ccc;
    color: white;
    border-radius: 16px;
    padding: 8px 12px;
    margin: 4px;
    font-size: 14px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1); /* Shadow for tags */
  }
  </style>
  