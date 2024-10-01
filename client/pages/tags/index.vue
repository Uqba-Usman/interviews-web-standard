<template>
    <div class="container">
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
              <tr 
              v-for="tag in tags"
              :key="tag.id"
              @click="goToTagDetail(tag.id)"
              class="clickable-row">
                <td>{{ tag.name }}</td>
                <td class="actions-column">
                  <!-- <NuxtLink :to="`/tags/show/${tag.id}`" class="btn-icon">
                    <img src="/open.png" alt="View" class="icon" />
                  </NuxtLink> -->
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
        tags: [],        
      };
    },
    created() {      
      this.fetchTagList();
    },
    methods: {
        goToTagDetail(tagId) {
      this.$router.push(`/tags/show/${tagId}`);
    },
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
  .tag-manager-title {
    font-size: 28px;
    color: #052e82;
    font-weight: bold;
  }
  
  .add-tag-btn {
    background-color: #052e82;
    border: none;
    color: white;
    padding: 10px 20px;
  }
  
  .add-tag-btn:hover {
    background-color: #041f66;
  }
  
  .actions-column {
    display: flex;
    align-items: center;
  }
  
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

.btn-icon:hover {
  background-color: #6c8fd3;
}

.btn-icon img {
  width: 20px;
  height: 20px;
  filter: invert(1); 
}

.view-tags-text {
  color: #041f66;
  text-decoration: underline;
  cursor: pointer;
  font-size: 14px;
}

.view-tags-text:hover {
  color: #305ccc; 
}

.clickable-row {
  cursor: pointer;
  transition: background-color 0.3s ease;
}
  
  .tag-list-card {
    border-radius: 12px;
  }
  
  .table th, .table td {
    vertical-align: middle;
  }
  
  .spinner-border {
    width: 3rem;
    height: 3rem;
  }
  
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
  }
  
  .modal-content {
    background-color: white;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    width: 400px;
  }
  
  .close-modal {
    position: absolute;
    top: 10px;
    right: 10px;
    background: none;
    border: none;
    font-size: 20px;
    cursor: pointer;
    color: #052e82;
  }
  </style>
  