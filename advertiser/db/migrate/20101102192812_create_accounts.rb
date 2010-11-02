class CreateAccounts < ActiveRecord::Migration
  def self.up
    create_table :accounts do |t|
      t.string :login
      t.string :password
      t.string :salt
      t.string :name
      t.text :desc

      t.timestamps
    end
  end

  def self.down
    drop_table :accounts
  end
end
