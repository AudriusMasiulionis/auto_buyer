/// <reference path="../pb_data/types.d.ts" />
migrate((db) => {
  const dao = new Dao(db);

  const admin = new Admin();
  admin.email = "test@test.com"
  admin.setPassword("1234567890")

  dao.saveAdmin(admin)
}, (db) => { // optional revert
  const dao = new Dao(db);

  try {
      const admin = dao.findAdminByEmail("test@test.com")

      dao.deleteAdmin(admin)
  } catch (_) { /* most likely already deleted */ }
})
