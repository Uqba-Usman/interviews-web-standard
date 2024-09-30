<template>
    <div class="container top-container">
      <div class="tag-detail-container">
        <div class="tag-header">
          <button @click="$router.go(-1)" class="back-icon">
            <img src="/back-icon.png" alt="Back Icon" />
          </button>
  
          <div class="tag-header-content">
            <h2 class="tag-title">{{ tag.name }}</h2>
          </div>
  
          <router-link :to="`/tags/edit/${tag.id}`" class="edit-icon">
            <img src="/edit.png" alt="Edit Icon" />
          </router-link>
        </div>
  
        <div class="card tag-card">
          <div class="card-body">
  
            <div class="tag-detail-section">
              <h4 class="detail-heading">Assigned Tasks</h4>
              <div class="tag-chips">
                <span >
                  No tasks assigned to this tag.
                </span>
              </div>
            </div>
          </div>
        </div>

      </div>
    </div>
  </template>
  
  <script>
  import Swal from "sweetalert2";
  import { getTag } from "~/services/tagService";
  
  export default {
    data() {
      return {
        tag: {
            id:"",
          name: "",
         
        },
        isSaving: false,
      };
    },
    created() {
      const id = this.$route.params.id;
      getTag(id)
        .then((response) => {
          const tagInfo = response.data;
          console.log("TAg:", tagInfo)
          this.tag.id = tagInfo.id;
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
  };
  </script>
  
  <style scoped>
  /* Main container adjustments */
  .top-container {
    margin-top: 5rem;
  }
  
  .tag-header {
    padding: 40px 20px;
    color: blue;
    border-radius: 8px 8px 0 0;
    text-align: center;
    position: relative;
  }
  
  /* Updated back button to match the style used in other screens */
  .back-icon {
    position: absolute;
    left: 20px;
    top: 50%;
    transform: translateY(-50%);
    font-size: 24px;
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
  
  .back-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
  .back-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  
  /* Updated edit icon to match the back button style */
  .edit-icon {
    position: absolute;
    right: 20px;
    top: 50%;
    transform: translateY(-50%);
    font-size: 24px;
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
  
  .edit-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
  .edit-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  
  .tag-header-content {
    max-width: 800px;
    margin: 0 auto;
  }
  
  .tag-title {
    font-size: 1.8rem;
    color: #052e82;
  }
  
  /* Card with shadow and without border */
  .tag-card {
    margin-top: -20px;
    border-radius: 12px;
    border: none;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  }
  
  .card-body {
    padding: 20px;
  }
  
  .tag-detail-section {
    margin-bottom: 20px;
  }
  
  .detail-heading {
    font-size: 20px;
    font-weight: bold;
    color: #052e82;
  }
  
  .tag-description {
    color: #333;
    font-size: 1.1rem;
    line-height: 1.6;
  }
  
  /* Tag chips updated to match the create screen */
  .tag-chips {
    display: flex;
    flex-wrap: wrap;
  }
  
  .tag-chip {
    background-color: #052e82;
    color: white;
    border-radius: 16px;
    padding: 8px 12px;
    margin: 4px;
    font-size: 14px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  }
  
  .tag-chip:hover {
    background-color: #2f61c5;
  }
  
  .clickable-chip {
    cursor: pointer;
  }
  
  </style>
  