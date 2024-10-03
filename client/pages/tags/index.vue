<template>
    <div class="container">
      <!-- Row containing title and button to add new tag -->
      <div class="row align-items-center mt-5 mb-3">
        <div class="col">
          <h2 class="tag-manager-title">Tag Manager</h2>
        </div>
        <div class="col text-end">
          <NuxtLink to="/tags/create" class="btn btn-primary add-tag-btn">
            +
          </NuxtLink>
        </div>
      </div>
  
      <!-- Card displaying the list of tags -->
      <div class="card tag-list-card">
        <div class="card-body">          
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Tag Name</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody v-if="!loading && tags.length">
              <!-- Loop through tags and display them -->
              <tr 
              v-for="tag in tags"
              :key="tag.id"
              @click="goToTagDetail(tag.id)"
              class="clickable-row">
                <td>{{ tag.name }}</td>
                <td class="actions-column">
                  <NuxtLink :to="`/tags/edit/${tag.id}`" class="btn-icon mx-1" @click.stop>
                    <img src="/edit.png" alt="Edit" class="icon" />
                  </NuxtLink>                  
                  <button @click="handleDelete(tag.id)" class="btn-icon" @click.stop>
                    <img src="/bin.png" alt="Delete" class="icon" />
                  </button>
                </td>
              </tr>
            </tbody>

          </table>
  
          <div v-if="loading" class="text-center my-5">
            <div class="spinner-border" role="status"></div>
          </div>
          <!-- Message when there are no tags available -->
          <div v-if="!loading && !tags.length" class="text-center my-5">
            <p>No tags available.</p>
          </div>
        </div>
      </div>
  
    </div>
  </template>
  
  <script>
  import { getTags, deleteTag } from "~/services/tagService";
  import Multiselect from "vue-multiselect";
  import Swal from "sweetalert2";
  
  export default {
    components: { Multiselect },
    data() {
      return {
        tags: [], 
        loading: true, 
      };
    },
    created() {      
      this.fetchTagList(); 
    },
    methods: {
      // Navigate to the detail page of the selected tag
      goToTagDetail(tagId) {
        this.$router.push(`/tags/show/${tagId}`);
      },
      // Fetch the list of tags from the API
      fetchTagList() {
        this.loading = true;
        getTags()
          .then((response) => {
            this.tags = response.data.$values.map((tag) => ({
              ...tag,
            }));
          })
          .catch((error) => console.error(error))
          .finally(() => (this.loading = false)); 
      },
      // Handle tag deletion
      handleDelete(id) {
        Swal.fire({
          title: "Are you sure?",
          text: "You can't undo this!",
          icon: "warning",
          showCancelButton: true,
          confirmButtonText: "Yes, delete it!",
        }).then((result) => {
          if (result.isConfirmed) {
            deleteTag(id) 
              .then(() => {
                Swal.fire("Deleted!", "Tag deleted successfully.", "success");
                this.fetchTagList(); 
              })
              .catch(() => Swal.fire("Error!", "Failed to delete tag.", "error"));
          }
        });
      },
    },
  };
  </script>
  
  <style scoped>
  /* Styling for the tag manager title */
  .tag-manager-title {
    font-size: 28px;
    color: #052e82;
    font-weight: bold;
  }
  
  /* Add new tag button styles */
  .add-tag-btn {
    background-color: #052e82;
    border: none;
    color: white;
    padding: 10px 20px;
  }
  
  /* Hover effect for add new tag button */
  .add-tag-btn:hover {
    background-color: #041f66;
  }
  
  /* Style for actions column with buttons */
  .actions-column {
    display: flex;
    align-items: center;
  }
  
  /* Styling for buttons with icons */
  .btn-icon {
    background-color: #041f66; 
    border: none;
    border-radius: 5px;
    padding: 8px;
    display: inline-flex;
    justify-content: center;
    align-items: center;
    width: 40px; 
    height: 40px;
    cursor: pointer;
  }
  
  /* Hover effect for icon buttons */
  .btn-icon:hover {
    background-color: #6c8fd3;
  }
  
  /* Icon styling for button images */
  .btn-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1); 
  }

  /* Styling for clickable table rows */
  .clickable-row {
    cursor: pointer;
    transition: background-color 0.3s ease;
  }

  /* Tag list card styling */
  .tag-list-card {
    border-radius: 12px;
  }
  
  /* Table cell alignment */
  .table th, .table td {
    vertical-align: middle;
  }

  /* Loading spinner size */
  .spinner-border {
    width: 3rem;
    height: 3rem;
  }
  </style>
